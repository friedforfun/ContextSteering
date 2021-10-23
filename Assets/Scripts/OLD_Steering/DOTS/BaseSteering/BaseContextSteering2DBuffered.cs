using Friedforfun.SteeringBehaviours.Core2D;
using Friedforfun.SteeringBehaviours.Utilities;
using UnityEngine;
using System.Collections.Concurrent;
using System.Linq;
using Friedforfun.SteeringBehaviours.Core;

namespace Friedforfun.SteeringBehaviours.Core2D.Buffered
{
    /// <summary>
    /// Base Class for context steering in 2D
    /// </summary>
    public class BaseContextSteering2DBuffered : MonoBehaviour
    {
        [Header("Behaviours")]
        public SteeringParameters steeringParameters;

        [Tooltip("Attractor and Repulsor strategies (directions in which we will move towards or away from.)")]
        public BufferedSteeringBehaviour[] SteeringBehaviours;

        [Tooltip("Masking strategies (directions in which to block movement).")]
        public SteeringMask[] SteeringMasks;

        [Tooltip("A strategy for combining steering and mask maps.")]
        protected ICombineContext ContextCombinator;

        [Tooltip("A strategy for selecting the final output direction from the context map.")]
        protected IDecideDirection DirectionDecider;

        private float resolutionAngle;
        private float[] contextMap;
        private float[] nextContextMap;
        private Vector3 lastVector;

        /// <summary>
        /// Gets the most desired direction to move in. Normalised.
        /// </summary>
        /// <returns></returns>
        public Vector3 MoveDirection()
        {
            return lastVector.normalized;
        }

        /// <summary>
        /// Should not be used directly as a move input.
        /// Get the direction and magnitude (not normalised) of the most desired direction, use depends on behaviour implementations.
        /// </summary>
        /// <returns></returns>
        public Vector3 MoveVector()
        {
            return lastVector;
        }

        //! TODO: Refactor awake out, ideally we want to update the context maps if the resolution is changed at runtime
        protected void Awake()
        {
            resolutionAngle = 360 / (float)steeringParameters.ContextMapResolution;

            foreach (BufferedSteeringBehaviour behaviour in SteeringBehaviours)
            {
                behaviour.InstantiateContextMap(steeringParameters);
            }

            foreach(SteeringMask mask in SteeringMasks)
            {
                mask.InstantiateMaskMap(steeringParameters);
            }

            SteeringScheduler s = FindObjectOfType<SteeringScheduler>();
            if (!s)
            {
                Debug.LogError("No SteeringScheduler found in the scene, this is required to use buffered steering");
            }
        }


        public void ScheduleJobs()
        {

            foreach (BufferedSteeringBehaviour b in SteeringBehaviours)
            {
                b.ScheduleJob();
            }
        }


        public void CompleteJobs()
        {
            ConcurrentBag<float[]> contextMaps = new ConcurrentBag<float[]>();

            foreach (BufferedSteeringBehaviour b in SteeringBehaviours)
            {
                b.CompleteJob();
                // load each new context map into a collection, and then combine context
                contextMaps.Add(b.GetContextMap());
            }

            float[] steeringMap = mergeMaps(contextMaps);

            // do same as above for masks
            //ConcurrentBag<float[]> maskMaps = new ConcurrentBag<float[]>();

            float[] maskMap = buildSteeringMasks();//mergeMaps(maskMaps);

            nextContextMap = ContextCombinator.CombineContext(steeringMap, maskMap);
            swap();
            lastVector = DirectionDecider.GetDirection(contextMap, lastVector);

        
        }


        private void swap()
        {
            float[] temp = contextMap;
            contextMap = nextContextMap;
            nextContextMap = temp;
        }



        /// <summary>
        /// Build a context mask of directions to block movement in this direction
        /// </summary>
        /// <returns></returns>
        private float[] buildSteeringMasks()
        {
            ConcurrentBag<float[]> contextMaps = new ConcurrentBag<float[]>();

            foreach (SteeringMask mask in SteeringMasks)
            {
                contextMaps.Add(mask.BuildMaskMap());
            }

            return mergeMaps(contextMaps);
        }

        /// <summary>
        /// Merge bag of context maps by summing each element
        /// </summary>
        /// <param name="maps"></param>
        /// <returns></returns>
        private float[] mergeMaps(ConcurrentBag<float[]> maps)
        {
            float[] contextMap = new float[steeringParameters.ContextMapResolution];
            for (int i = 0; i < steeringParameters.ContextMapResolution; i++)
            {
                contextMap[i] = 0f;
            }

            foreach (float[] map in maps)
            {
                var newMap = contextMap.Zip(map, (x, y) => x + y);
                contextMap = newMap.ToArray();
            }

            return contextMap;
        }

    }


}


using UnityEngine;
using System.Collections.Concurrent;
using System.Linq;
using Friedforfun.SteeringBehaviours.Utilities;
using Friedforfun.SteeringBehaviours.Core;


namespace Friedforfun.SteeringBehaviours.Core2D
{
    /// <summary>
    /// Base Class for context steering in 2D
    /// </summary>
    public class BaseContextSteering2D : MonoBehaviour
    {
        [Header("Behaviours")]
        public SteeringParameters steeringParameters;

        [Tooltip("Attractor and Repulsor strategies (directions in which we will move towards or away from.)")]
        public SteeringBehaviour[] SteeringBehaviours; 

        [Tooltip("Masking strategies (directions in which to block movement).")]
        public SteeringMask[] SteeringMasks;

        [Tooltip("A strategy for combining steering and mask maps.")]
        protected ICombineContext ContextCombinator;

        [Tooltip("A strategy for selecting the final output direction from the context map.")]
        protected IDecideDirection DirectionDecider;

        private float resolutionAngle; // The angle between each vector represented by the context map.
        private float[] contextMap; // The weights of each direction in the context map itself
        private Vector3 lastVector; // The previous vector output.

        /// <summary>
        /// Gets the most desired direction to move in. Normalised.
        /// </summary>
        /// <returns></returns>
        public Vector3 MoveDirection()
        {
            contextMap = ContextCombinator.CombineContext(buildSteeringBehaviours(), buildSteeringMasks());
            lastVector = DirectionDecider.GetDirection(contextMap, lastVector);
            return lastVector.normalized;
        }

        /// <summary>
        /// Should not be used directly as a move input.
        /// Get the direction and magnitude (not normalised) of the most desired direction, use depends on behaviour implementations.
        /// </summary>
        /// <returns></returns>
        public Vector3 MoveVector()
        {
            contextMap = ContextCombinator.CombineContext(buildSteeringBehaviours(), buildSteeringMasks());
            lastVector = DirectionDecider.GetDirection(contextMap, lastVector);
            return lastVector;
        }

        //! TODO: Refactor awake out, ideally we want to update the context maps if the resolution is changed at runtime
        protected void Awake()
        {
            resolutionAngle = 360 / (float)steeringParameters.ContextMapResolution;

            foreach (SteeringBehaviour behaviour in SteeringBehaviours)
            {
                behaviour.InstantiateContextMap(steeringParameters);
            }

            foreach (SteeringMask mask in SteeringMasks)
            {
                mask.InstantiateMaskMap(steeringParameters);
            }
        }


        /// <summary>
        /// Builds all steering behaviours and sums each weight elementwise, to determine the most desirable direction to move in.
        /// </summary>
        /// <returns></returns>
        private float[] buildSteeringBehaviours()
        {
            ConcurrentBag<float[]> contextMaps = new ConcurrentBag<float[]>();

            foreach (SteeringBehaviour behaviour in SteeringBehaviours)
            {
                contextMaps.Add(behaviour.BuildContextMap());
            }

            return mergeMaps(contextMaps);
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


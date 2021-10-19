using System.Collections;
using System.Linq;
using UnityEngine;
using Friedforfun.SteeringBehaviours.Core;
using System.Collections.Concurrent;

namespace Friedforfun.SteeringBehaviours.PlanarMovement
{
    public class PlanarSteeringController : CoreSteeringController
    {
        [Header("Behaviours")]
        public PlanarSteeringParameters steeringParameters;

        //[Tooltip("Attractor and Repulsor strategies (directions in which we will move towards or away from.)")]
        protected PlanarSteeringBehaviour[] SteeringBehaviours;

        //[Tooltip("Masking strategies (directions in which to block movement).")]
        protected PlanarSteeringMask[] SteeringMasks;

        protected float[] contextMap; // The weights of each direction in the context map itself


        protected float[] MergeSteeringBehaviours()
        {
            ConcurrentBag<float[]> contextMaps = new ConcurrentBag<float[]>();

            foreach (PlanarSteeringBehaviour behaviour in SteeringBehaviours)
            {
                contextMaps.Add(behaviour.GetContextMap());
            }

            return mergeMaps(contextMaps);
        }

        protected void UpdateOutput()
        {
            // --------------- Until masks are implemented ---------------
            //contextMap = ContextCombinator.CombineContext(MergeSteeringBehaviours(), new float[steeringParameters.ContextMapResolution]);
            contextMap = MergeSteeringBehaviours();
            // -----------------------------------------------------------
            Debug.Log("Updating Output");
            Debug.Log($"Vec before: {outputVector}");
            outputVector = DirectionDecider.GetDirection(contextMap, outputVector);
            Debug.Log($"Vec after: {outputVector}");
        }

        /// <summary>
        /// Get all the jobs from the steering behaviours
        /// </summary>
        /// <returns></returns>
        public DotToVecJob[] GetJobs()
        {
            DotToVecJob[] jobs = new DotToVecJob[SteeringBehaviours.Length];
            for (int i = 0; i < SteeringBehaviours.Length; i++)
            {
                jobs[i] = SteeringBehaviours[i].GetJob();
            }

            return jobs;
        }
        
        protected virtual void Awake()
        {
            outputVector = steeringParameters.InitialVector;

            SteeringBehaviours = gameObject.GetComponentsInChildren<PlanarSteeringBehaviour>();

            if (SteeringBehaviours != null)
            {
                foreach (PlanarSteeringBehaviour behaviour in SteeringBehaviours)
                {
                    Debug.Log($"Controller init -> {behaviour.BehaviourName}");
                    behaviour.InstantiateContextMap(steeringParameters);
                }
            }


            SteeringMasks = gameObject.GetComponentsInChildren<PlanarSteeringMask>();

            if (SteeringMasks != null)
            {
                foreach (PlanarSteeringMask mask in SteeringMasks)
                {
                    mask.InstantiateMaskMap(steeringParameters);
                }
            }

        }


        /// <summary>
        /// Merge collection of context maps by summing each element
        /// </summary>
        /// <param name="maps"></param>
        /// <returns></returns>
        protected float[] mergeMaps(ConcurrentBag<float[]> maps)
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


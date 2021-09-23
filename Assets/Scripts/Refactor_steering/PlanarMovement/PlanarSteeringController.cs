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
        private PlanarSteeringBehaviour[] SteeringBehaviours;

        //[Tooltip("Masking strategies (directions in which to block movement).")]
        private PlanarSteeringMask[] SteeringMasks;

        protected float[] contextMap; // The weights of each direction in the context map itself

        protected float[] buildSteeringBehaviours()
        {
            ConcurrentBag<float[]> contextMaps = new ConcurrentBag<float[]>();

            foreach (PlanarSteeringBehaviour behaviour in SteeringBehaviours)
            {
                contextMaps.Add(behaviour.BuildContextMap());
            }

            return mergeMaps(contextMaps);
        }

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
            SteeringBehaviours = gameObject.GetComponentsInChildren<PlanarSteeringBehaviour>();

            foreach (PlanarSteeringBehaviour behaviour in SteeringBehaviours)
            {
                behaviour.InstantiateContextMap(steeringParameters);
            }

            SteeringMasks = gameObject.GetComponentsInChildren<PlanarSteeringMask>();

            foreach (PlanarSteeringMask mask in SteeringMasks)
            {
                mask.InstantiateMaskMap(steeringParameters);
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


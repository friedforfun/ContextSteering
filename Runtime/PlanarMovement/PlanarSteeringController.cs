using System.Collections;
using System.Linq;
using UnityEngine;
using Friedforfun.ContextSteering.Core;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Friedforfun.ContextSteering.PlanarMovement
{
    public class PlanarSteeringController : CoreSteeringController
    {
        [Header("Behaviours")]
        public PlanarSteeringParameters steeringParameters;

        //[Tooltip("Attractor and Repulsor strategies (directions in which we will move towards or away from.)")]
        protected PlanarSteeringBehaviour[] SteeringBehaviours;
        private PlanarSteeringBehaviour[] GetBehaviours() => SteeringBehaviours; // Used for unit tests

        //[Tooltip("Masking strategies (directions in which to block movement).")]
        protected PlanarSteeringMask[] SteeringMasks;

        protected float[] contextMap; // The weights of each direction in the context map itself
        private float[] GetContextMap() => contextMap; // Used for unit tests

        /// <summary>
        /// Combines all steering behaviours into a single weight array, summing them point wise. 
        /// </summary>
        /// <returns></returns>
        protected float[] MergeSteeringBehaviours()
        {
            List<float[]> contextMaps = new List<float[]>();

            foreach (PlanarSteeringBehaviour behaviour in SteeringBehaviours)
            {
                contextMaps.Add(behaviour.GetContextMap());
            }

            return mergeMaps(contextMaps);
        }

        /// <summary>
        /// Combines all masks into a single weight array, by summing them point wise.
        /// </summary>
        /// <returns></returns>
        protected float[] MergeMasks()
        {
            List<float[]> masks = new List<float[]>();

            foreach (PlanarSteeringMask mask in SteeringMasks)
            {
                masks.Add(mask.GetMaskMap());
            }

            return mergeMaps(masks);
        }

        /// <summary>
        /// Updates the output vector computed by evaluating all behaviours and masks.
        /// </summary>
        public void UpdateOutput()
        {
            contextMap = ContextCombinator.CombineContext(MergeSteeringBehaviours(), MergeMasks());

            outputVector = DirectionDecider.GetDirection(contextMap);
        }

        /// <summary>
        /// Get all the jobs from the steering behaviours, includes both behaviours and masks.
        /// </summary>
        /// <returns></returns>
        public DotToVecJob[] GetJobs()
        {
            DotToVecJob[] jobs = new DotToVecJob[SteeringBehaviours.Length + SteeringMasks.Length];
            for (int i = 0; i < SteeringBehaviours.Length; i++)
            {
                jobs[i] = SteeringBehaviours[i].GetJob();
            } 
            for ( int i = SteeringBehaviours.Length; i < SteeringBehaviours.Length + SteeringMasks.Length; i++)
            {
                jobs[i] = SteeringMasks[i - SteeringBehaviours.Length].GetJob();
            } 

            return jobs;
        }
        
        public virtual void Awake()
        {
            outputVector = Vector3.zero;

            SteeringBehaviours = gameObject.GetComponentsInChildren<PlanarSteeringBehaviour>();

            if (SteeringBehaviours != null)
            {
                foreach (PlanarSteeringBehaviour behaviour in SteeringBehaviours)
                {
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
            contextMap = new float[steeringParameters.ContextMapResolution];

        }


        /// <summary>
        /// Merge collection of context maps by summing each element
        /// </summary>
        /// <param name="maps"></param>
        /// <returns></returns>
        protected float[] mergeMaps(List<float[]> maps)
        {
            float[] contextMap = new float[steeringParameters.ContextMapResolution];

            foreach (float[] map in maps)
            {
                var newMap = contextMap.Zip(map, (x, y) => x + y);
                contextMap = newMap.ToArray();
            }

            return contextMap;
        }
    }
}


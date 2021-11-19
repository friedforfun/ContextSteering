using Unity.Collections;
using System.Collections;
using Unity.Jobs;
using Unity.Burst;
using UnityEngine;
using Friedforfun.ContextSteering.Core;
using System;

namespace Friedforfun.ContextSteering.PlanarMovement
{
    public class SelfSchedulingPlanarController : PlanarSteeringController
    {
        [Tooltip("Number of times per second this controller will update behaviour values.")]
        [Range(1, 100)]
        public int TicksPerSecond = 10;

        [Tooltip("The direction selection algorithm")]
        [SerializeField]
        DirectionSelectorTypes directionSelector = DirectionSelectorTypes.BASIC;

        [Tooltip("The minimal acceptable dot product between the last tick direction and this one.")]
        [Range(-1f, 1f)]
        [SerializeField]
        float MinDotPerTick = 0.3f;


        JobHandle[] Handles;
        bool JobRunning = false;

        /// <summary>
        /// Choose and instantiate direction selection algorithm
        /// </summary>
        private void SelectDirection()
        {
            switch(directionSelector)
            {
                case DirectionSelectorTypes.BASIC:
                    DirectionDecider = new BasicPlanarDirectionPicker(true, steeringParameters);
                    break;

                case DirectionSelectorTypes.WITH_INERTIA:
                    DirectionDecider = new PlanarDirectionSimpleSmoothing(MinDotPerTick, steeringParameters);
                    break;
            }
        }

        public override void Awake()
        {
            base.Awake();
            ContextCombinator = new BasicContextCombinator();
            SelectDirection();
            StartCoroutine(CycleWork());
        }

        public void OnDisable()
        {
            if (JobRunning)
                CompleteWork();
        }

        private void ScheduleWork()
        {
            JobRunning = true;
            var jobs = GetJobs();
            Handles = new JobHandle[jobs.Length];

            for (int i = 0; i < Handles.Length; i++)
            {
                Handles[i] = jobs[i].Schedule();
            }
        }

        private void CompleteWork()
        {
            JobRunning = false;
            foreach (JobHandle h in Handles)
            {
                h.Complete();
            }

            foreach (PlanarSteeringBehaviour psb in SteeringBehaviours)
            {
                psb.Swap();
            }

            foreach (PlanarSteeringMask psm in SteeringMasks)
            {
                psm.Swap();
            }
        }

        private IEnumerator CycleWork()
        {
            yield return new WaitForEndOfFrame();
            yield return new WaitForSeconds(UnityEngine.Random.Range(0f, 1f));
            for (; ; )
            {
                yield return new WaitForSeconds(1f / TicksPerSecond);
                ScheduleWork();

                yield return new WaitUntil(() => Array.TrueForAll(Handles, value => value.IsCompleted));
                CompleteWork();
                UpdateOutput();

            }

        }
    }
}


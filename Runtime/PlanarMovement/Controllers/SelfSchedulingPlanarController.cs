using Unity.Collections;
using System.Collections;
using Unity.Jobs;
using Unity.Burst;
using UnityEngine;
using Friedforfun.SteeringBehaviours.Core;
using System;

namespace Friedforfun.SteeringBehaviours.PlanarMovement
{
    public class SelfSchedulingPlanarController : PlanarSteeringController
    {
        [Tooltip("Number of times per second this controller will update behaviour values.")]
        [Range(1, 100)]
        public int TicksPerSecond = 10;

        [SerializeField]
        float MaxRotationDotPerTick = 0.3f;

        JobHandle[] Handles;
        bool JobRunning = false;

        public override void Awake()
        {
            base.Awake();
            ContextCombinator = new BasicContextCombinator();
            DirectionDecider = new PlanarDirectionSimpleSmoothing(MaxRotationDotPerTick, steeringParameters);
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
            yield return new WaitForSeconds(UnityEngine.Random.Range(0f, 0.5f));
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


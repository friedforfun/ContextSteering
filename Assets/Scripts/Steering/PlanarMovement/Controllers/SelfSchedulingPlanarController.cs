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

        JobHandle[] Handles;
        bool JobRunning = false;

        public SelfSchedulingPlanarController()
        {
            ContextCombinator = new BasicContextCombinator();
            DirectionDecider = new BasicPlanarDirectionPicker(true, steeringParameters);
        }

        public override void Awake()
        {
            base.Awake();
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
            Handles = new JobHandle[SteeringBehaviours.Length];

            for (int i = 0; i < SteeringBehaviours.Length; i++)
            {
                Handles[i] = SteeringBehaviours[i].GetJob().Schedule();
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
        }

        private IEnumerator CycleWork()
        {
            yield return new WaitForEndOfFrame();
            for (; ; )
            {
                yield return new WaitForSeconds(1 / TicksPerSecond);
                ScheduleWork();

                yield return new WaitUntil(() => Array.TrueForAll(Handles, value => value.IsCompleted));
                CompleteWork();
                UpdateOutput();

            }

        }
    }
}


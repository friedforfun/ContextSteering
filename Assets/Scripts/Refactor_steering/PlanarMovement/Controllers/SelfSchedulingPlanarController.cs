using Unity.Collections;
using System.Collections;
using Unity.Jobs;
using Unity.Burst;
using UnityEngine;
using Friedforfun.SteeringBehaviours.Core;
using Friedforfun.SteeringBehaviours.Core2D;

namespace Friedforfun.SteeringBehaviours.PlanarMovement
{
    public class SelfSchedulingPlanarController : PlanarSteeringController
    {
        [Tooltip("Number of times per second this controller will update behaviour values.")]
        [Range(1, 100)]
        public int TicksPerSecond = 10;

        JobHandle[] Handles;

        public SelfSchedulingPlanarController()
        {
            ContextCombinator = new BasicContextCombinator();
            DirectionDecider = new BasicDirectionPicker(true, steeringParameters);
        }

        protected override void Awake()
        {
            base.Awake();
            //jobHandles = new NativeArray<DotToVecJob>(SteeringBehaviours.Length, Allocator.Persistent);
            StartCoroutine(CycleWork());
        }


        private void ScheduleWork()
        {
            Handles = new JobHandle[SteeringBehaviours.Length];
            for (int i = 0; i < SteeringBehaviours.Length; i++)
            {
                Handles[i] = SteeringBehaviours[i].GetJob().Schedule();
            }
        }

        private void CompleteWork()
        {
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

                //yield return new WaitUntil(() => Schedulerhandle.IsCompleted);
                CompleteWork();
                UpdateOutput();

            }

        }
    }
}


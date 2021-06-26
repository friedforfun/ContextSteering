using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using Unity.Burst;
using UnityEngine;
using Friedforfun.SteeringBehaviours.Core2D;
using Friedforfun.SteeringBehaviours.Utilities;

namespace Friedforfun.SteeringBehaviours.Core2D.Buffered
{
    public class BufferedDotToTag : BufferedSteeringBehaviourFromTags
    {
        [Header("Behaviour Properties")]
        [SerializeField] SteerDirection direction = SteerDirection.ATTRACT;
        [SerializeField] float Weight = 1f;

        private NativeArray<float> nextMap;
        private NativeArray<Vector3> targetPositions;
        private JobHandle jobHandle;


        private void Awake()
        {
            nextMap = new NativeArray<float>(resolution, Allocator.Persistent);
        }

        private void OnDisable()
        {
            nextMap.Dispose();
        }


        public override void ScheduleJob()
        {
            //nextMap = new NativeArray<float>(resolution, Allocator.TempJob);
            Vector3[] targetArr = getTargetVectors();

            targetPositions = new NativeArray<Vector3>(targetArr.Length, Allocator.TempJob);

            for (int i = 0; i < targetArr.Length; i++)
            {
                targetPositions[i] = targetArr[i];
                if (MapOperations.VectorToTarget(transform.position, targetPositions[i]).magnitude < Range)
                    Debug.DrawLine(transform.position, targetPositions[i], Color.red);
            }

            DotToVecJob job = new DotToVecJob()
            {
                targets = targetPositions,
                position = transform.position,
                range = Range,
                weight = Weight,
                angle = resolutionAngle,
                Weights = nextMap,
                direction = direction,
                scaled = false,
                invertScale = 0
            };

            jobHandle = job.Schedule();
        }

        public override void CompleteJob()
        {
            jobHandle.Complete();

            float[] next = new float[resolution];
            for (int i = 0; i < nextMap.Length; i++)
            {
                next[i] = nextMap[i];
            }


            targetPositions.Dispose();

            steeringMap = next;
        }



    }
}
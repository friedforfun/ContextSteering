using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using Friedforfun.SteeringBehaviours.Core2D;

namespace Friedforfun.SteeringBehaviours.Core2D.Buffered
{
    public class BufferedDotToTagScaledWeight : BufferedSteeringBehaviourFromTags
    {
        [Header("Behaviour Properties")]
        [SerializeField] SteerDirection direction = SteerDirection.ATTRACT;
        [SerializeField] bool InvertScale = true;
        [SerializeField] float Weight = 1f;

        private float invertScalef { get { return InvertScale ? 1f : 0f; } }

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

            Vector3[] targetArr = getTargetVectors();

            targetPositions = new NativeArray<Vector3>(targetArr.Length, Allocator.TempJob);

            for (int i = 0; i < targetArr.Length; i++)
            {
                targetPositions[i] = targetArr[i];
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
                scaled = true,
                invertScale = invertScalef
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
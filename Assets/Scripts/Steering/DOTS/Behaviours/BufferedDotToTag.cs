using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using Unity.Burst;
using UnityEngine;

public class BufferedDotToTag : BufferedSteeringBehaviour
{
    [Header("Behaviour Properties")]
    [SerializeField] SteerDirection direction = SteerDirection.ATTRACT;
    [SerializeField] float Weight = 1f;
    [SerializeField] string[] Tags;

    private NativeArray<float> nextMap;
    private NativeArray<Vector3> targetPositions;
    private JobHandle jobHandle;


    private Vector3[] getTargetVectors()
    {
        int oldLen = 0;
        Vector3[] targets = null;
        foreach (string tag in Tags)
        {
            var y = GameObject.FindGameObjectsWithTag(tag);
            if (targets == null)
            {
                targets = new Vector3[y.Length];

            }
            else
            {
                oldLen = targets.Length;
                Array.Resize(ref targets, targets.Length + y.Length);
            }
            for (int i = oldLen; i < targets.Length; i++)
            {
                targets[i] = y[i - oldLen].transform.position;
            }
        }
        return targets;
    }


    public override void ScheduleJob()
    {
        nextMap = new NativeArray<float>(resolution, Allocator.TempJob);
        Vector3[] targetArr = getTargetVectors();

        targetPositions = new NativeArray<Vector3>(targetArr.Length, Allocator.TempJob);

        for (int i = 0; i < targetArr.Length; i++)
        {
            targetPositions[i] = targetArr[i];
        }

        BufferedDotToTagJob job = new BufferedDotToTagJob()
        {
            targets = targetPositions,
            position = transform.position,
            range = Range,
            weight = Weight,
            angle = resolutionAngle,
            Weights = nextMap,
            direction = direction
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

        nextMap.Dispose();
        targetPositions.Dispose();



        steeringMap = next;
    }

    [BurstCompile] 
    public struct BufferedDotToTagJob : IJob
    {
        [ReadOnly]
        public NativeArray<Vector3> targets;

        public Vector3 position;
        public float range, weight, angle;

        public NativeArray<float> Weights;

        public SteerDirection direction;

        public void Execute()
        {
            foreach (Vector3 target in targets)
            {
                Vector3 targetVector = MapOperations.VectorToTarget(position, target);
                float distance = targetVector.magnitude;
                if (distance < range)
                {
                    Vector3 mapVector = Vector3.forward;
                    for (int i = 0; i < Weights.Length; i++)
                    {
                        Weights[i] += Vector3.Dot(mapVector, targetVector.normalized) * weight;
                        mapVector = Quaternion.Euler(0f, angle, 0f) * mapVector;
                    }
                }
            }

            if (direction == SteerDirection.REPULSE)
                Weights = MapOperations.ReverseMap(Weights);

        }
    }

}

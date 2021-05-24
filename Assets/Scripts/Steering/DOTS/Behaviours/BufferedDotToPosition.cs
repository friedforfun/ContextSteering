using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using Unity.Burst;
using UnityEngine;

public class BufferedDotToPosition : BufferedSteeringBehaviour
{
    [Header("Behaviour Properties")]
    [SerializeField] SteerDirection direction = SteerDirection.ATTRACT;
    [SerializeField] float Weight = 1f;
    [SerializeField] Transform[] Positions;

    private NativeArray<float> nextMap;
    private NativeArray<Vector3> targetPositions;
    private JobHandle jobHandle;


    private Vector3[] getTargetVectors()
    {
        Vector3[] targets = new Vector3[Positions.Length];
        for (int i = 0; i < Positions.Length; i++)
        {
            targets[i] = Positions[i].position;
        }
        return targets;
    }


    public void UpdatePositions(Transform[] positions)
    {
        Positions = positions;
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
            invertScale = 0f
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

}

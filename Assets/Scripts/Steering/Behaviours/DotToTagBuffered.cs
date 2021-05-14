using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Jobs;
using UnityEngine;
using Unity.Collections;

public class DotToTagBuffered : SteeringBehaviour
{
    [Header("Behaviour Properties")]
    [SerializeField] SteerDirection direction = SteerDirection.ATTRACT;
    [SerializeField] float weight = 1f;
    [SerializeField] string[] Tags;
    [SerializeField] float TargetUpdateInterval;

    private float lastUpdate = 0f;
    private float[] next;

    private NativeArray<float> jobContextMap;
    private JobHandle jobHandle;
    private bool jobRunning = false;

    private void swap()
    {
        float[] temp = steeringMap;
        steeringMap = next;
        next = temp;
    }

    private void Update()
    {
        float current = Time.time;
        
        if (current - lastUpdate < TargetUpdateInterval && !jobHandle.IsCompleted)
            return;

        if (jobRunning)
        {
            finishJob();
        }

        jobContextMap = new NativeArray<float>(resolution, Allocator.TempJob);
        Vector3[] targets = getTargetVectors();

        scheduletContextMap(targets);
        jobRunning = true;
        lastUpdate = Time.time;
    }

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

    /// <summary>
    /// Finishes job, and swaps buffer
    /// </summary>
    private void finishJob()
    {
        jobRunning = false;
        jobHandle.Complete();
        next = new float[resolution];
        for (int i = 0; i < jobContextMap.Length; i++)
        {
            next[i] = jobContextMap[i];
        }

        jobContextMap.Dispose();

        if (direction == SteerDirection.REPULSE)
            next = MapOperations.ReverseMap(next);

        swap();
    }

    /// <summary>
    /// creates and schedules build context map job
    /// </summary>
    /// <param name="targets"></param>
    private void scheduletContextMap(Vector3[] targets)
    {
        // use unity jobs - create a struct containing an array of target position vectors, this agents position vector and the context map
        jobContextMap = new NativeArray<float>(resolution, Allocator.TempJob);

        NativeArray<Vector3> targetArr = new NativeArray<Vector3>(targets.Length, Allocator.TempJob);
        for (int i = 0; i < targets.Length; i++)
        {
            targetArr[i] = targets[i];
        }

        BuildContextMapJob mapJob = new BuildContextMapJob(gameObject.transform.position, targetArr, Range, weight, resolutionAngle, jobContextMap);
        jobHandle = mapJob.Schedule();
    }

    public override float[] BuildContextMap()
    {
        return steeringMap;
    }

    private struct BuildContextMapJob : IJob
    {
        [ReadOnly]
        NativeArray<Vector3> targets;

        Vector3 position;
        float range, weight, angle;

        NativeArray<float> result;

        public BuildContextMapJob(Vector3 position, NativeArray<Vector3> targets, float range, float weight, float angle, NativeArray<float> result)
        {
            this.targets = targets;
            this.position = position;
            this.range = range;
            this.weight = weight;
            this.angle = angle;
            this.result = result;
        }

        public void Execute()
        {
            foreach (Vector3 target in targets)
            {
                Vector3 targetVector = MapOperations.VectorToTarget(position, target);
                float distance = targetVector.magnitude;
                if (distance < range)
                {
                    Vector3 mapVector = Vector3.forward;
                    for (int i = 0; i < result.Length; i++)
                    {
                        result[i] += Vector3.Dot(mapVector, targetVector.normalized) * weight;
                        mapVector = Quaternion.Euler(0f, angle, 0f) * mapVector;
                    }
                }
            }
        }
    }

}


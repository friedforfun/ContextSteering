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

    private float[] next;
    private void swap()
    {
        float[] temp = steeringMap;
        steeringMap = next;
        next = temp;
    }

    private void constructContextMap()
    {
        int oldLen = 0;
        Vector3[] targets = null;
        next = new float[resolution];
        foreach (string tag in Tags)
        {
            var y = GameObject.FindGameObjectsWithTag(tag);
            if (targets == null)
            {
                targets = new Vector3[y.Length];

            } else
            {
                oldLen = targets.Length;
                Array.Resize(ref targets, targets.Length + y.Length);
            }
            for (int i = oldLen; i < targets.Length; i++)
            {
                targets[i] = y[i - oldLen].transform.position;
            }
        }

        // use unity jobs - create a struct containing an array of target position vectors, this agents position vector and the context map
        NativeArray<float> result = new NativeArray<float>(resolution, Allocator.TempJob);

        BuildContextMapJob mapJob = new BuildContextMapJob(gameObject.transform.position, targets, Range, weight, resolutionAngle, result);
        JobHandle handle = mapJob.Schedule();

        handle.Complete();

        for (int i = 0; i < result.Length; i++)
        {
            next[i] = result[i];
        }

        result.Dispose();

        if (direction == SteerDirection.REPULSE)
            next = MapOperations.ReverseMap(next);

        swap();
    }
    public override float[] BuildContextMap()
    {
        return steeringMap;
    }

}

public struct BuildContextMapJob : IJob
{
    Vector3[] targets;
    Vector3 position;
    float range, weight, angle;

    NativeArray<float> result;

    public BuildContextMapJob(Vector3 position, Vector3[] targets, float range, float weight, float angle, NativeArray<float> result)
    {
        this.position = position;
        this.targets = targets;
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

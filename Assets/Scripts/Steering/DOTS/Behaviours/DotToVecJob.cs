using Unity.Jobs;
using Unity.Burst;
using UnityEngine;
using Unity.Collections;
using Friedforfun.SteeringBehaviours.Utilities;

namespace Friedforfun.SteeringBehaviours.Core2D.Buffered
{

    [BurstCompile]
    public struct DotToVecJob : IJob
    {
        [ReadOnly]
        public NativeArray<Vector3> targets;

        public bool scaled;

        public Vector3 position;
        public float range, weight, angle, invertScale;

        public NativeArray<float> Weights;

        public SteerDirection direction;

        public void Execute()
        {
            for (int i = 0; i < Weights.Length; i++)
            {
                Weights[i] = 0f;
            }

            foreach (Vector3 target in targets)
            {
                Vector3 targetVector = MapOperations.VectorToTarget(position, target);
                float distance = targetVector.magnitude;
                if (distance < range)
                {
                    Vector3 mapVector = Vector3.forward;
                    for (int i = 0; i < Weights.Length; i++)
                    {
                        if (!scaled)
                            Weights[i] += Vector3.Dot(mapVector, targetVector.normalized) * weight;
                        else
                            Weights[i] += Vector3.Dot(mapVector, targetVector.normalized) * Mathf.Abs((invertScale * 1f) - (distance / range)) * weight;
                        mapVector = Quaternion.Euler(0f, angle, 0f) * mapVector;
                    }
                }
            }

            if (direction == SteerDirection.REPULSE)
                Weights = MapOperations.ReverseMap(Weights);

        }
    }
}
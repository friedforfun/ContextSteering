using Unity.Jobs;
using Unity.Burst;
using UnityEngine;
using Unity.Collections;
using Friedforfun.ContextSteering.Utilities;

namespace Friedforfun.ContextSteering.Core
{

    [BurstCompile]
    public struct DotToVecJob : IJob
    {
        [ReadOnly]
        public NativeArray<Vector3> targets;

        public bool scaled;

        public Vector3 my_position;
        public float range, weight, angle, invertScale;

        public RotationAxis axis;

        public NativeArray<float> Weights;

        public SteerDirection direction;


        private float sqrRange;
        public void Execute()
        {
            

            for (int i = 0; i < Weights.Length; i++)
            {
                Weights[i] = 0f;
            }

            if (!scaled)
                standardMap();
            else
                scaledMap();

            if (direction == SteerDirection.REPULSE)
                Weights = MapOperations.ReverseMap(Weights);

        }

        private void standardMap()
        {
            sqrRange = range * range;

            foreach (Vector3 target in targets)
            {
                Vector3 targetVector = MapOperations.VectorToTarget(my_position, target);
                float distance = targetVector.sqrMagnitude;
                if (distance < sqrRange)
                {
                    Vector3 mapVector = InitialVector();
                    for (int i = 0; i < Weights.Length; i++)
                    {

                        Weights[i] += Vector3.Dot(mapVector, targetVector.normalized) * weight;

                        mapVector = MapOperations.RotateAroundAxis(axis, angle) * mapVector;
                    }
                }
            }
        }

        private void scaledMap()
        {
            foreach (Vector3 target in targets)
            {
                Vector3 targetVector = MapOperations.VectorToTarget(my_position, target);
                float distance = targetVector.magnitude;
                if (distance < range)
                {
                    Vector3 mapVector = InitialVector();
                    for (int i = 0; i < Weights.Length; i++)
                    {
                        Weights[i] += Vector3.Dot(mapVector, targetVector.normalized) * Mathf.Abs((invertScale * 1f) - (distance / range)) * weight;
                        mapVector = MapOperations.RotateAroundAxis(axis, angle) * mapVector;
                    }
                }
            }
        }

        private Vector3 InitialVector()
        {
            switch (axis)
            {
                case RotationAxis.YAxis:
                    return Vector3.forward;
                case RotationAxis.ZAxis:
                    return Vector3.up;
                case RotationAxis.XAxis:
                    return Vector3.forward;
                default:
                    throw new System.NotImplementedException();
            }
        }
 

    }
}
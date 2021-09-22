using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using Friedforfun.SteeringBehaviours.Core;

namespace Friedforfun.SteeringBehaviours.PlanarMovement
{
    public class DotToTransform : PlanarSteeringBehaviour
    {
        [SerializeField] Transform[] Positions;

        private NativeArray<float> nextMap;

        public override float[] BuildContextMap()
        {

            return steeringMap;
        }

        private void Swap()
        {
            float[] next = new float[steeringParameters.ContextMapResolution];
            for (int i = 0; i < nextMap.Length; i++)
            {
                next[i] = nextMap[i];
            }

            steeringMap = next;
        }

        private Vector3[] getPositionVectors()
        {
            Vector3[] targets = new Vector3[Positions.Length];
            for (int i = 0; i < Positions.Length; i++)
            {
                targets[i] = Positions[i].position;
            }
            return targets;
        }

        public DotToVecJob GetJob()
        {
            nextMap = new NativeArray<float>(steeringParameters.ContextMapResolution, Allocator.TempJob);
            Vector3[] targetArr = getPositionVectors();

            var targetPositions = new NativeArray<Vector3>(targetArr.Length, Allocator.TempJob);

            for (int i = 0; i < targetArr.Length; i++)
            {
                targetPositions[i] = targetArr[i];
            }

            return new DotToVecJob()
            {
                targets = targetPositions,
                my_position = transform.position,
                range = Range,
                weight = Weight,
                angle = steeringParameters.ResolutionAngle,
                Weights = nextMap,
                direction = Direction,
                scaled = ScaleOnDistance,
                invertScale = invertScalef

            };
        }

    }

}

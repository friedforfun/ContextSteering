using Friedforfun.SteeringBehaviours.Utilities;
using UnityEngine;

namespace Friedforfun.SteeringBehaviours.Core2D
{
    public class DotToLayerMask : SteeringMask
    {
        [SerializeField] LayerMask LayersForSteeringMask;
        [SerializeField] float weight = 1f;

        public override float[] BuildMaskMap()
        {
            maskMap = new float[resolution];

            Collider[] checkLayers = Physics.OverlapSphere(transform.position, Range, LayersForSteeringMask);
            if (checkLayers != null)
            {
                foreach (Collider collision in checkLayers)
                {
                    Vector3 direction = MapOperations.VectorToTarget(transform.position, collision.ClosestPoint(transform.position));
                    Vector3 mapVector = InitialVector;
                    for (int i = 0; i < maskMap.Length; i++)
                    {
                        maskMap[i] += Vector3.Dot(mapVector, direction.normalized) * weight;
                        mapVector = rotateAroundAxis(resolutionAngle) * mapVector;
                    }
                }
            }
            return maskMap;
        }
    }
}


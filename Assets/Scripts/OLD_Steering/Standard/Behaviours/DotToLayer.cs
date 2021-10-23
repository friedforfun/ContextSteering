using UnityEngine;
using Friedforfun.SteeringBehaviours.Utilities;

namespace Friedforfun.SteeringBehaviours.Core2D
{
    public class DotToLayer : SteeringBehaviour
    {
        [SerializeField] SteerDirection direction = SteerDirection.ATTRACT;
        [SerializeField] LayerMask Layers;
        [SerializeField] float weight = 1f;

        public override float[] BuildContextMap()
        {
            steeringMap = new float[resolution];

            Collider[] checkLayers = Physics.OverlapSphere(transform.position, Range, Layers);
            if (checkLayers != null)
            {
                foreach (Collider collision in checkLayers)
                {
                    Vector3 direction = MapOperations.VectorToTarget(transform.position, collision.ClosestPoint(transform.position));
                    Vector3 mapVector = InitialVector;
                    for (int i = 0; i < steeringMap.Length; i++)
                    {
                        steeringMap[i] += Vector3.Dot(mapVector, direction.normalized) * weight;
                        mapVector = rotateAroundAxis(resolutionAngle) * mapVector;
                    }
                }
            }

            if (direction == SteerDirection.REPULSE)
                steeringMap = MapOperations.ReverseMap(steeringMap);

            return steeringMap;
        }
    }
}


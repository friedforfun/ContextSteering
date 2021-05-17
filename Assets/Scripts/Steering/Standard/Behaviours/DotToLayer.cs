using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
                Vector3 mapVector = Vector3.forward;
                for (int i = 0; i < steeringMap.Length; i++)
                {
                    steeringMap[i] += Vector3.Dot(mapVector, direction.normalized) * weight;
                    mapVector = Quaternion.Euler(0f, resolutionAngle, 0f) * mapVector;
                }
            }
        }

        if (direction == SteerDirection.REPULSE)
            steeringMap = MapOperations.ReverseMap(steeringMap);

        return steeringMap;
    }
}

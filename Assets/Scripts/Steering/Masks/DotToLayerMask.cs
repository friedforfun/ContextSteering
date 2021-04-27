using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
                Vector3 mapVector = Vector3.forward;
                for (int i = 0; i < maskMap.Length; i++)
                {
                    maskMap[i] += Vector3.Dot(mapVector, direction.normalized) * weight;
                    mapVector = Quaternion.Euler(0f, resolutionAngle, 0f) * mapVector;
                }
            }
        }
        return maskMap;
    }
}

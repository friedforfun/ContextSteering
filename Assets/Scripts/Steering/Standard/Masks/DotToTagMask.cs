using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DotToTagMask : SteeringMask
{
    [SerializeField] string[] Tags;
    [SerializeField] float weight = 1f;

    public override float[] BuildMaskMap()
    {
        maskMap = new float[resolution];

        foreach (string tag in Tags)
        {
            foreach (GameObject target in GameObject.FindGameObjectsWithTag(tag))
            {
                Vector3 direction = MapOperations.VectorToTarget(gameObject, target);
                float distance = direction.magnitude;
                if (distance < Range)
                {
                    Vector3 mapVector = Vector3.forward;
                    for (int i = 0; i < maskMap.Length; i++)
                    {
                        maskMap[i] += Vector3.Dot(mapVector, direction.normalized) * weight;
                        mapVector = Quaternion.Euler(0f, resolutionAngle, 0f) * mapVector;
                    }
                }
            }
            
        }
        
        return maskMap;
    }
}

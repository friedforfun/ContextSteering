using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DotToTagRepulsor : SteeringBehaviour
{
    [Header("Repulsor Properties")]
    [SerializeField] float weight = 1f;
    [SerializeField] string[] RepulseTags;


    public override float[] BuildContextMap()
    {
        steeringMap = new float[resolution];
        foreach (string tag in RepulseTags)
        {
            foreach (GameObject target in GameObject.FindGameObjectsWithTag(tag))
            {
                Vector3 targetVector = MapOperations.VectorToTarget(gameObject, target);
                float distance = targetVector.magnitude;
                if (distance < Range)
                {
                    Vector3 mapVector = Vector3.forward;
                    for (int i = 0; i < steeringMap.Length; i++)
                    {
                        // 
                        steeringMap[i] += Vector3.Dot(mapVector, targetVector.normalized) * weight;
                        mapVector = Quaternion.Euler(0f, resolutionAngle, 0f) * mapVector;
                    }
                }
            }
        }
        steeringMap = MapOperations.ReverseMap(steeringMap);
        return steeringMap;
    }



}

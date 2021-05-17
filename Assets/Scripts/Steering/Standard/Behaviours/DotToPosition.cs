using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DotToPosition : SteeringBehaviour
{
    [Header("Behaviour Properties")]
    [SerializeField] SteerDirection direction = SteerDirection.ATTRACT;
    [SerializeField] float weight = 1f;
    [SerializeField] Transform[] positions;


    public override float[] BuildContextMap()
    {
        steeringMap = new float[resolution];
        foreach (Transform location in positions)
        {

            Vector3 targetVector = MapOperations.VectorToTarget(gameObject.transform.position, location.position);
            float distance = targetVector.magnitude;
            if (distance < Range)
            {
                Vector3 mapVector = Vector3.forward;
                for (int i = 0; i < steeringMap.Length; i++)
                {
                    steeringMap[i] += Vector3.Dot(mapVector, targetVector.normalized) * weight;
                    mapVector = Quaternion.Euler(0f, resolutionAngle, 0f) * mapVector;
                }
            }
            
        }

        if (direction == SteerDirection.REPULSE)
            steeringMap = MapOperations.ReverseMap(steeringMap);

        return steeringMap;
    }



}

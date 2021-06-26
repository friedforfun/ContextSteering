using UnityEngine;
using Friedforfun.SteeringBehaviours.Utilities;

namespace Friedforfun.SteeringBehaviours.Core2D
{
    public class DotToTag : SteeringBehaviour
    {
        [Header("Behaviour Properties")]
        [SerializeField] SteerDirection direction = SteerDirection.ATTRACT;
        [SerializeField] float weight = 1f;
        [SerializeField] string[] Tags;


        public override float[] BuildContextMap()
        {
            steeringMap = new float[resolution];
            foreach (string tag in Tags)
            {
                // Inefficient - should cache tagged gameobjects
                foreach (GameObject target in GameObject.FindGameObjectsWithTag(tag))
                {
                    Vector3 targetVector = MapOperations.VectorToTarget(gameObject, target);
                    float distance = targetVector.magnitude;
                    if (distance < Range)
                    {
                        Vector3 mapVector = InitialVector;
                        for (int i = 0; i < steeringMap.Length; i++)
                        {
                            steeringMap[i] += Vector3.Dot(mapVector, targetVector.normalized) * weight;
                            mapVector = rotateAroundAxis(resolutionAngle) * mapVector;
                        }
                    }
                }
            }

            if (direction == SteerDirection.REPULSE)
                steeringMap = MapOperations.ReverseMap(steeringMap);

            return steeringMap;
        }



    }
}
using Friedforfun.SteeringBehaviours.Utilities;
using UnityEngine;

namespace Friedforfun.SteeringBehaviours.Core2D
{
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
                        Vector3 mapVector = InitialVector;
                        for (int i = 0; i < maskMap.Length; i++)
                        {
                            maskMap[i] += Vector3.Dot(mapVector, direction.normalized) * weight;
                            mapVector = rotateAroundAxis(resolutionAngle) * mapVector;
                        }
                    }
                }
            
            }
        
            return maskMap;
        }
    }
}


using Friedforfun.SteeringBehaviours.Utilities;
using UnityEngine;

namespace Friedforfun.SteeringBehaviours.Core2D
{
    public enum SteerDirection
    {
        ATTRACT,
        REPULSE
    }

    /// <summary>
    /// Build attractor or repulsor context map with scaling based on the distance to the target
    /// </summary>
    public class DotToTagScaledWeight : SteeringBehaviour
    {
        [Header("Behaviour Properties")]
        [SerializeField] SteerDirection direction = SteerDirection.ATTRACT;
        [SerializeField] bool InvertScale = true;
        [SerializeField] float weight = 1f;
        [SerializeField] string[] Tags;

        private float invertScalef { get { return InvertScale ? 1f : 0f; } }

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
                    if (distance <= Range)
                    {
                        Debug.DrawLine(transform.position, target.transform.position, Color.red);
                        Vector3 mapVector = InitialVector;
                        for (int i = 0; i < steeringMap.Length; i++)
                        {
                            // Branchless scale inversion
                            steeringMap[i] += Vector3.Dot(mapVector, targetVector.normalized) * Mathf.Abs((invertScalef * 1f) - (distance / Range)) * weight;
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

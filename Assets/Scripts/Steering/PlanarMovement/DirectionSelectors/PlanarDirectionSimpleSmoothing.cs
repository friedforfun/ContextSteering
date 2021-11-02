using UnityEngine;
using Friedforfun.SteeringBehaviours.Core;

namespace Friedforfun.SteeringBehaviours.PlanarMovement
{
    ///<Summary>
    /// This strategy picks a direction, that direction is limited by the maximum change in direction each time the method is called.
    /// Should be called at a fixed rate.
    ///</Summary>
    public class PlanarDirectionSimpleSmoothing : IDecideDirection
    {
        private float MaxDot;
        private Vector3 lastVector = Vector3.zero;
        public PlanarDirectionSimpleSmoothing(float MaxDot)
        {
            this.MaxDot = Mathf.Clamp(MaxDot, -1, 1);
        }

        public Vector3 GetDirection(float[] contextMap)
        {
            float resolutionAngle = 360 / (float)contextMap.Length;

            float maxValue = 0f;
            int maxIndex = 0;
            for (int i = 0; i < contextMap.Length; i++)
            {
                if (contextMap[i] > maxValue)
                {

                    maxValue = contextMap[i];
                    maxIndex = i;
                }
            }

            Vector3 direction = Vector3.forward * maxValue;

            if (maxValue == 0f)
            {
                return lastVector; // Keep last direction if no better direction is found
            }

            Vector3 nextVector = Quaternion.Euler(0, resolutionAngle * maxIndex, 0) * direction;
            float dot = Mathf.Clamp(Vector3.Dot(lastVector.normalized, nextVector.normalized), -1f, 1f);

            // next direction is within direction change
            if (dot < MaxDot)
                return nextVector;

            float desiredAngleRad = Mathf.Acos(MaxDot);

            lastVector = Vector3.RotateTowards(lastVector.normalized, nextVector.normalized, desiredAngleRad, 1);
            return lastVector;
        }
    }
}

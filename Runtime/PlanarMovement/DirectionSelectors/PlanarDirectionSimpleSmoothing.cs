using UnityEngine;
using Friedforfun.ContextSteering.Core;
using Friedforfun.ContextSteering.Utilities;

namespace Friedforfun.ContextSteering.PlanarMovement
{
    ///<Summary>
    /// This strategy picks a direction, that direction is limited by the maximum change in direction each time the method is called.
    /// Should be called at a fixed rate.
    ///</Summary>
    public class PlanarDirectionSimpleSmoothing : IDecideDirection
    {
        private float MinDot;
        private Vector3 lastVector = Vector3.zero;
        private PlanarSteeringParameters steeringParams;
        public PlanarDirectionSimpleSmoothing(float MinDot, PlanarSteeringParameters steeringParameters)
        {
            this.MinDot = Mathf.Clamp(MinDot, -1, 1);
            steeringParams = steeringParameters;
        }

        public Vector3 GetDirection(float[] contextMap)
        {
            float resolutionAngle = steeringParams.ResolutionAngle;

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

            Vector3 nextVector = MapOperations.RotateAroundAxis(steeringParams.ContextMapRotationAxis, resolutionAngle * maxIndex) * direction;
            float dot = Mathf.Clamp(Vector3.Dot(lastVector.normalized, nextVector.normalized), -1f, 1f);

            // next direction is within direction change
            if (dot > MinDot)
            {
                lastVector = nextVector;
                return lastVector;
            }


            float desiredAngleRad = Mathf.Acos(MinDot);

            lastVector = Vector3.RotateTowards(lastVector, nextVector, desiredAngleRad, 1);
            return lastVector;
        }
    }
}

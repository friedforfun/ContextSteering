using UnityEngine;
using Friedforfun.SteeringBehaviours.Core;
using Friedforfun.SteeringBehaviours.Utilities;

namespace Friedforfun.SteeringBehaviours.PlanarMovement
{
    ///<Summary>
    /// Most basic direction selection, pick the highest weighted direction.
    ///</Summary>
    public class BasicPlanarDirectionPicker : IDecideDirection
    {
        private bool allowVectorZero = true;
        private PlanarSteeringParameters steeringParams;

        public BasicPlanarDirectionPicker(bool allowZero, PlanarSteeringParameters steeringParameters)
        {
            this.allowVectorZero = allowZero;
            steeringParams = steeringParameters;
        }

        public Vector3 GetDirection(float[] contextMap, Vector3 lastVector)
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
                if (allowVectorZero)
                    return Vector3.zero;

                return lastVector; // Keep last direction if no better direction is found
            }

            if (steeringParams == null)
                return Quaternion.Euler(0, resolutionAngle * maxIndex, 0) * direction;

            return MapOperations.RotateAroundAxis(steeringParams.ContextMapRotationAxis, resolutionAngle * maxIndex) * direction;
        }
    }
}

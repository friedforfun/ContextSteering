using UnityEngine;
using Friedforfun.ContextSteering.Core;
using Friedforfun.ContextSteering.Utilities;

namespace Friedforfun.ContextSteering.PlanarMovement
{
    ///<Summary>
    /// Most basic direction selection, pick the highest weighted direction.
    ///</Summary>
    public class BasicPlanarDirectionPicker : IDecideDirection
    {
        private bool allowVectorZero = true;
        private PlanarSteeringParameters steeringParams;
        private Vector3 lastVector = Vector3.zero;

        public BasicPlanarDirectionPicker(bool allowZero, PlanarSteeringParameters steeringParameters)
        {
            this.allowVectorZero = allowZero;
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
                if (allowVectorZero)
                    return Vector3.zero;

                return lastVector; // return last direction if no better direction is found
            }

            if (steeringParams == null)
                throw new UnassignedReferenceException("The direction selection algorithm does not know what axis to use, check constructor (PlanarSteeringParameters).");
            
            // Update cache & return new direction.
            lastVector = MapOperations.RotateAroundAxis(steeringParams.ContextMapRotationAxis, resolutionAngle * maxIndex) * direction;
            return lastVector;
        }
    }
}

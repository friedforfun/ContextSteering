using UnityEngine;
using Friedforfun.SteeringBehaviours.Core;

namespace Friedforfun.SteeringBehaviours.Core2D
{

    ///<Summary>
    /// Most basic direction selection, pick the highest weighted direction.
    ///</Summary>
    public class BasicDirectionPicker : IDecideDirection
    {
        private bool allowVectorZero = true;

        public BasicDirectionPicker(bool allowZero)
        {
            this.allowVectorZero = allowZero;
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


            return Quaternion.Euler(0, resolutionAngle * maxIndex, 0) * direction;
        }
    }

    ///<Summary>
    /// This strategy picks a direction, that direction is limited by the maximum change in direction each time the method is called.
    /// Should be called at a fixed rate.
    ///</Summary>
    public class DirectionSimpleSmoothing : IDecideDirection
    {
        private float MaxDot;

        public DirectionSimpleSmoothing(float MaxDot)
        {
            this.MaxDot = Mathf.Clamp(MaxDot, -1, 1);
        }

        public Vector3 GetDirection (float[] contextMap, Vector3 lastVector)
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

            return Vector3.RotateTowards(lastVector.normalized, nextVector.normalized, desiredAngleRad, 1);
        }
    }


    public class BackProjectedDirectionPicker : IDecideDirection
    {
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

            // highest adjacent index to the max
            // need to handle array out of bounds issue (maxIndex is 0 || maxIndex is array length)
            int secondaryIndex = contextMap[maxIndex - 1] > contextMap[maxIndex + 1] ? maxIndex - 1 : maxIndex + 1; 

        
            if (secondaryIndex > maxIndex) {
                // Compute the gradient of the line between secondaryIndex and its next weight,
                // Compute gradient of maxIndex and its previous weight
            }
            if (secondaryIndex < maxIndex) {
                // Compute the gradient of the line between secondaryIndex and its previous weight,
                // Compute gradient of maxIndex and its next weight
            }
            if (secondaryIndex == maxIndex) {
                // return direction halfway between secondary and max
            }
        


            Vector3 direction = Vector3.forward * maxValue;

            if (maxValue == 0f)
            {
                return lastVector; // Keep last direction if no better direction is found
            }


            return Quaternion.Euler(0, resolutionAngle * maxIndex, 0) * direction;
        }

        private float GetGradient(float a, float b) 
        {
       
            return 0f;
        }
    }

}

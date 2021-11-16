using UnityEngine;
using Friedforfun.ContextSteering.Core;

namespace Friedforfun.ContextSteering.PlanarMovement
{
    public class BackProjectedDirectionPicker : IDecideDirection
    {
        private Vector3 lastVector = Vector3.zero;

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

            // highest adjacent index to the max
            // need to handle array out of bounds issue (maxIndex is 0 || maxIndex is array length)
            int secondaryIndex = contextMap[maxIndex - 1] > contextMap[maxIndex + 1] ? maxIndex - 1 : maxIndex + 1;

 
            if (secondaryIndex > maxIndex)
            {
                // Compute the gradient of the line between secondaryIndex and its next weight,
                // Compute gradient of maxIndex and its previous weight
            }
            if (secondaryIndex < maxIndex)
            {
                // Compute the gradient of the line between secondaryIndex and its previous weight,
                // Compute gradient of maxIndex and its next weight
            }
            if (secondaryIndex == maxIndex)
            {
                // return direction halfway between secondary and max
            }



            Vector3 direction = Vector3.forward * maxValue;

            if (maxValue == 0f)
            {
                return lastVector; // Keep last direction if no better direction is found
            }

            throw new System.NotImplementedException();
            //return Quaternion.Euler(0, resolutionAngle * maxIndex, 0) * direction;
        }

        private float GetGradient(float a, float b)
        {
            throw new System.NotImplementedException();
            //return 0f;
        }
    }

}

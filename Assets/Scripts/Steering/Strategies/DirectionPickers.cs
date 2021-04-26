using UnityEngine;

public class BasicDirectionPicker : IDecideDirection
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

        Vector3 direction = Vector3.forward * maxValue;

        if (maxValue == 0f)
        {
            return lastVector; // Keep last direction if no better direction is found
        }


        return Quaternion.Euler(0, resolutionAngle * maxIndex, 0) * direction;
    }
}

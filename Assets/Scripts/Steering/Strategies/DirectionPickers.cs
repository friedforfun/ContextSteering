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
        if (dot < MaxDot)
            return nextVector;

        float desiredAngleRad = Mathf.Acos(MaxDot);
        return Vector3.RotateTowards(lastVector.normalized, nextVector.normalized, desiredAngleRad, 1);
    }
}

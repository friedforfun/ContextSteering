using UnityEngine;
public interface IDecideDirection
{
    Vector3 GetDirection(float[] contextMap, float resolutionAngle, Vector3 lastVector);
}
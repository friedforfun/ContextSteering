using UnityEngine;
public interface IDecideDirection
{
    Vector3 GetDirection(float[] contextMap, Vector3 lastVector);
}
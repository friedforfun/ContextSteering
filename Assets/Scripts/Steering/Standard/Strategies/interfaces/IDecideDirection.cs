using UnityEngine;

namespace Friedforfun.SteeringBehaviours.Core2D
{
    public interface IDecideDirection
    {
        Vector3 GetDirection(float[] contextMap, Vector3 lastVector);
    }
}
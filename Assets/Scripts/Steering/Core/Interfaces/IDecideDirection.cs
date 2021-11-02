using UnityEngine;

namespace Friedforfun.SteeringBehaviours.Core
{
    public interface IDecideDirection
    {
        Vector3 GetDirection(float[] contextMap);
    }
}
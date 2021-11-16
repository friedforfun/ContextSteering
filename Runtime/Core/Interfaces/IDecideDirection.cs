using UnityEngine;

namespace Friedforfun.ContextSteering.Core
{
    public interface IDecideDirection
    {
        Vector3 GetDirection(float[] contextMap);
    }
}
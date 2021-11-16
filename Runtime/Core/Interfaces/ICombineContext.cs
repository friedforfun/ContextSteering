namespace Friedforfun.ContextSteering.Core
{
    public interface ICombineContext
    {
        float[] CombineContext(float[] steeringMap, float[] maskMap);
    }

}


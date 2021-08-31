namespace Friedforfun.SteeringBehaviours.Core
{
    public interface ICombineContext
    {
        float[] CombineContext(float[] steeringMap, float[] maskMap);
    }

}


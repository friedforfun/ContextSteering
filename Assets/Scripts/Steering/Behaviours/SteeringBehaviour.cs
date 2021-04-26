using UnityEngine;

public abstract class SteeringBehaviour: MonoBehaviour
{
    protected int resolution; // The number of directions we compute weights for.
    protected float[] steeringMap; // The map of weights, each element represents our degree of interest in the direction that element corresponds to.
    protected float resolutionAngle; // Each point is seperated by a some degrees rotation (360/steeringMap.Length)

    /// <summary>
    /// Instantiates the context map weights and computes the angle between each direction
    /// </summary>
    /// <param name="resolution"></param>
    public void InstantiateContextMap(int resolution)
    {
        this.resolution = resolution;
        resolutionAngle = 360 / (float)resolution;
        steeringMap = new float[resolution];        
    }

    /// <summary>
    /// Build a context map where the index of the float defines the direction we wish to move, the size of the scalar defines how much we want to move in a direction
    /// </summary>
    /// <returns></returns>
    public abstract float[] BuildContextMap();
}

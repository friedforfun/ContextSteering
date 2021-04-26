using UnityEngine;

public abstract class SteeringMask : MonoBehaviour
{
    protected int resolution; // The number of directions we compute weights for.
    protected float[] maskMap; // The map of weights, each element represents our degree of interest in the direction that element corresponds to.
    protected float resolutionAngle; // Each point is seperated by a some degrees rotation (360/contextMap.Length)

    /// <summary>
    /// Instantiates the mask map weights and computes the angle between each direction
    /// </summary>
    /// <param name="resolution"></param>
    public void InstantiateMaskMap(int resolution)
    {
        this.resolution = resolution;
        resolutionAngle = 360 / (float)resolution;
        maskMap = new float[resolution];
    }

    /// <summary>
    /// Build a mask map where the index of the float defines the direction we wish to move, the size of the scalar defines how much we want to move in a direction
    /// </summary>
    /// <returns></returns>
    public abstract float[] BuildMaskMap();
}

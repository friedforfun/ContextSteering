public interface ISteeringBehaviour
{
    /// <summary>
    /// Build a context map where the index of the float defines the direction we wish to move, the size of the scalar defines how much we want to move in a direction
    /// </summary>
    /// <returns></returns>
    float[] BuildContextMap();
}

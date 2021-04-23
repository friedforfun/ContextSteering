public interface ISteeringMask
{
    /// <summary>
    /// Essentially the same as a steering behaviour context map but it will be used as a mask for valid directions to move
    /// </summary>
    /// <returns></returns>
    float[] BuildContextMask();
}


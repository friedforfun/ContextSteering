using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public abstract class SteeringMask : MonoBehaviour
{
    [SerializeField] protected float Range;
    protected int resolution; // The number of directions we compute weights for.
    protected float[] maskMap; // The map of weights, each element represents our degree of interest in the direction that element corresponds to.
    protected float resolutionAngle; // Each point is seperated by a some degrees rotation (360/contextMap.Length)

    [Header("Debug")]
    [SerializeField] private bool ShowDebug = false;
    [SerializeField] private float MapSize = 2f;
    [SerializeField] private Color DebugColor = Color.red;

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

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {

        if (!ShowDebug || maskMap is null || maskMap.Length == 0)
        {
            return;
        }

        Vector3 position = transform.position;
        Handles.DrawWireDisc(position, Vector3.up, Range);

        position = new Vector3(position.x, position.y + 0.1f, position.z);
        Vector3 direction = Vector3.forward;

        foreach (float weight in MapOperations.NormaliseMap(maskMap, MapSize))
        {
            Gizmos.color = DebugColor;
            Gizmos.DrawRay(transform.position, direction * weight);
            direction = Quaternion.Euler(0f, resolutionAngle, 0) * direction;
        }
        Handles.color = DebugColor;
        Handles.DrawWireDisc(position, Vector3.up, MapSize);

    }
#endif

}

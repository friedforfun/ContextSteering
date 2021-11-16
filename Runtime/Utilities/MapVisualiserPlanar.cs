#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace Friedforfun.ContextSteering.Utilities
{
    [System.Serializable]
    public class PlanarMapVisualiserParameters
    {
        [Tooltip("Enable/Disable debug visuals")]
        [SerializeField] public bool ShowDebug = false;
        [Tooltip("The Radius of the debug map to be drawn (useful for overlapping maps).")]
        [SerializeField] public float MapSize = 2f;
        [Tooltip("The colour of the debug map that will be drawn.")]
        [SerializeField] public Color DebugColor = Color.red;
    }

    public static class MapVisualiserPlanar 
    {


    #if UNITY_EDITOR
        /// <summary>
        /// Call this inside OnDrawGizmos in the steering behaviour or mask you wish to visualise.
        /// </summary>
        public static void InDrawGizmos(PlanarMapVisualiserParameters MapParams, float[] contextMap, float range, float resolutionAngle, Transform transform)
        {
            bool ShowDebug = MapParams.ShowDebug;
            float MapSize = MapParams.MapSize;
            Color DebugColor = MapParams.DebugColor;

            if (!ShowDebug || contextMap is null || contextMap.Length == 0)
            {
                return;
            }

            Vector3 position = transform.position;
            Handles.DrawWireDisc(position, Vector3.up, range);

            position = new Vector3(position.x, position.y + 0.1f, position.z);
            Vector3 direction = Vector3.forward;


            foreach (float weight in MapOperations.NormaliseMap(contextMap, MapSize))
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

}


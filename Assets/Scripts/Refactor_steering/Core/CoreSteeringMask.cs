using UnityEngine;
using Friedforfun.SteeringBehaviours.Utilities;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Friedforfun.SteeringBehaviours.Core
{
    public abstract class CoreSteeringMask : MonoBehaviour
    {
        [Tooltip("Range at which the mask has an effect.")]
        [SerializeField] protected float Range;
        
        protected float[] maskMap; //The map of mask weights, each element represents our degree of interest in the direction that element corresponds to.
        protected float resolutionAngle; // Each point is seperated by a some degrees rotation (360/contextMap.Length)
        protected RotationAxis ContextMapAxis;
        protected Vector3 InitialVector;

        [SerializeField] public MapVisualiserParameters MapDebugger;
        private MapVisualiser2D MapDebugVis = new MapVisualiser2D();


#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            MapDebugVis.InDrawGizmos(MapDebugger, maskMap, Range, resolutionAngle, transform);
        }
#endif
    }
}


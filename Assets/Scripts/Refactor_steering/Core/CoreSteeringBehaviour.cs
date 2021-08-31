using UnityEngine;
using Friedforfun.SteeringBehaviours.Utilities;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Friedforfun.SteeringBehaviours.Core
{
    /// <summary>
    /// All steering behaviours should inherit from this base class. And implement either ICreateSteeringJob or IDefineSteering
    /// </summary> 
    public abstract class CoreSteeringBehaviour : MonoBehaviour
    {
        [Tooltip("Range at which the behaviour has an effect.")]
        [SerializeField] protected float Range;
        protected int resolution { get; private set; } // The number of directions we compute weights for.
        protected float[] steeringMap = null; // The map of weights, each element represents our degree of interest in the direction that element corresponds to.
        protected float resolutionAngle { get; private set; } // Each point is seperated by a some degrees rotation (360/steeringMap.Length)
        protected RotationAxis ContextMapAxis;
        protected Vector3 InitialVector;



        [SerializeField] public MapVisualiserParameters MapDebugger;
        private MapVisualiser2D MapDebugVis = new MapVisualiser2D();

        /// <summary>
        /// Instantiates the context map weights and computes the angle between each direction
        /// </summary>
        /// <param name="steeringParameters"></param>
        public void InstantiateContextMap(SteeringParameters steeringParameters)
        {
            InitialVector = steeringParameters.InitialVector;
            ContextMapAxis = steeringParameters.ContextMapRotationAxis;
            resolution = steeringParameters.ContextMapResolution;
            resolutionAngle = 360 / (float)resolution;
            steeringMap = new float[resolution];
        }



        protected Quaternion rotateAroundAxis(float resolutionAngle)
        {
            return MapOperations.rotateAroundAxis(ContextMapAxis, resolutionAngle);
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            MapDebugVis.InDrawGizmos(MapDebugger, steeringMap, Range, resolutionAngle, transform);
        }
#endif

    }
}



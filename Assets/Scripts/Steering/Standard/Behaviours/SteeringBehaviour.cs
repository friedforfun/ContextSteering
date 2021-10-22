using UnityEngine;
using Friedforfun.SteeringBehaviours.Utilities;
using Friedforfun.SteeringBehaviours.Core;

namespace Friedforfun.SteeringBehaviours.Core2D
{
    /// <summary>
    /// All steering behaviours should inherit from this base class.
    /// </summary> 
    public abstract class SteeringBehaviour : MonoBehaviour
    {
        [Tooltip("Range at which the behaviour has an effect.")]
        [SerializeField] protected float Range;
        protected int resolution { get; private set; } // The number of directions we compute weights for.
        protected float[] steeringMap = null; // The map of weights, each element represents our degree of interest in the direction that element corresponds to.
        protected float resolutionAngle;//{ get; private set; } // Each point is seperated by a some degrees rotation (360/steeringMap.Length)
        protected RotationAxis ContextMapAxis;
        protected Vector3 InitialVector;

        [SerializeField] public PlanarMapVisualiserParameters MapDebugger;

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

        /// <summary>
        /// Build a context map where the index of the float defines the direction we wish to move, the size of the scalar defines how much we want to move in a direction
        /// </summary>
        /// <returns></returns>
        public abstract float[] BuildContextMap();


        protected Quaternion rotateAroundAxis(float resolutionAngle)
        {
            return MapOperations.RotateAroundAxis(ContextMapAxis, resolutionAngle);
        }

#if UNITY_EDITOR
        protected virtual void OnDrawGizmos()
        {
            MapVisualiserPlanar.InDrawGizmos(MapDebugger, steeringMap, Range, resolutionAngle, transform);
        }
#endif

    }
}


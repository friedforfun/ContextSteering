using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Friedforfun.SteeringBehaviours.Core;
using Friedforfun.SteeringBehaviours.Utilities;

namespace Friedforfun.SteeringBehaviours.PlanarMovement
{
    public abstract class PlanarSteeringBehaviour : CoreSteeringBehaviour<PlanarSteeringParameters>
    {
        protected int resolution { get; private set; } // The number of directions we compute weights for.
        protected float[] steeringMap = null; // The map of weights, each element represents our degree of interest in the direction that element corresponds to.
        protected float resolutionAngle { get; private set; } // Each point is seperated by a some degrees rotation (360/steeringMap.Length)
        protected RotationAxis ContextMapAxis;

        [SerializeField] public PlanarMapVisualiserParameters MapDebugger;
        private MapVisualiserPlanar MapDebugVis = new MapVisualiserPlanar();

        /// <summary>
        /// Instantiates the context map weights and computes the angle between each direction
        /// </summary>
        /// <param name="steeringParameters"></param>
        public override void InstantiateContextMap(PlanarSteeringParameters steeringParameters)
        {
            // refactor this data into reference to steeringParameters class
            InitialVector = steeringParameters.InitialVector;
            ContextMapAxis = steeringParameters.ContextMapRotationAxis;
            resolution = steeringParameters.ContextMapResolution;
            resolutionAngle = steeringParameters.ResolutionAngle;
            // ---- ^^^ this data ^^^ ----

            steeringMap = new float[resolution]; // re-instantiate this array when resolution changes
        }

        /// <summary>
        /// Build a context map where the index of the float defines the direction we wish to move, the size of the scalar defines how much we want to move in a direction
        /// </summary>
        /// <returns></returns>
        public abstract float[] BuildContextMap();

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

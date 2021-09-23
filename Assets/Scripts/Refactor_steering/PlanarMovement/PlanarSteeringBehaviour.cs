using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Friedforfun.SteeringBehaviours.Core;
using Friedforfun.SteeringBehaviours.Utilities;

namespace Friedforfun.SteeringBehaviours.PlanarMovement
{
    public abstract class PlanarSteeringBehaviour : CoreSteeringBehaviour<PlanarSteeringParameters>
    {
        [SerializeField] protected SteerDirection Direction = SteerDirection.ATTRACT;
        [SerializeField] protected float Weight = 1f;
        [SerializeField] protected bool ScaleOnDistance = false;
        [SerializeField] bool InvertScale = true;

        protected float invertScalef { get { return InvertScale ? 1f : 0f; } }

        protected float[] steeringMap = null; // The map of weights, each element represents our degree of interest in the direction that element corresponds to.
        protected PlanarSteeringParameters steeringParameters;

        [SerializeField] public PlanarMapVisualiserParameters MapDebugger;
        private MapVisualiserPlanar MapDebugVis = new MapVisualiserPlanar();

        /// <summary>
        /// Instantiates the context map weights and computes the angle between each direction
        /// </summary>
        /// <param name="steeringParameters"></param>
        public override void InstantiateContextMap(PlanarSteeringParameters steeringParameters)
        {
            this.steeringParameters = steeringParameters;
            this.steeringParameters.OnResolutionChange += MapResolutionChangeHandler;
            steeringMap = new float[steeringParameters.ContextMapResolution];
        }

        /// <summary>
        /// Runs when the context map resolution is changed at runtime
        /// </summary>
        private void MapResolutionChangeHandler()
        {
            steeringMap = new float[steeringParameters.ContextMapResolution];
        }

        /// <summary>
        /// Build a context map where the index of the float defines the direction we wish to move, the size of the scalar defines how much we want to move in a direction
        /// </summary>
        /// <returns></returns>
        public abstract float[] BuildContextMap();

        public abstract DotToVecJob GetJob();


        protected virtual void OnDestroy()
        {
            steeringParameters.OnResolutionChange -= MapResolutionChangeHandler;
        }

#if UNITY_EDITOR
        protected virtual void OnDrawGizmos()
        {
            MapDebugVis.InDrawGizmos(MapDebugger, steeringMap, Range, steeringParameters.ResolutionAngle, transform);
        }
#endif

    }

}

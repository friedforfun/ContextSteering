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
        /// Return a context map where the index of the float defines the direction we wish to move, the size of the scalar defines how much we want to move in a direction
        /// </summary>
        /// <returns></returns>
        public virtual float[] GetContextMap()
        {
            return steeringMap;
        }

        /// <summary>
        /// Get the job required to update the steering map, to be scheduled
        /// </summary>
        /// <returns>DotToVecJob ready to be scheduled</returns>
        public abstract DotToVecJob GetJob();

        /// <summary>
        /// swaps the completed job data into the steering map, run this when the jobs are complete before scheduling the next job
        /// </summary>
        public abstract void Swap();


        protected virtual void OnDestroy()
        {
            steeringParameters.OnResolutionChange -= MapResolutionChangeHandler;
        }

#if UNITY_EDITOR
        protected virtual void OnDrawGizmos()
        {
            if (steeringMap != null && MapDebugger != null)
                MapDebugVis.InDrawGizmos(MapDebugger, steeringMap, Range, steeringParameters.ResolutionAngle, transform);
        }
#endif

    }

}

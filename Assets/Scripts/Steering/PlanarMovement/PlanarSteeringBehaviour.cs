using UnityEngine;
using Friedforfun.SteeringBehaviours.Core;
using Friedforfun.SteeringBehaviours.Utilities;
using Unity.Collections;

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

        private NativeArray<float> nextMap;
        private NativeArray<Vector3> targetPositions;

        /// <summary>
        /// Instantiates the context map weights and computes the angle between each direction
        /// </summary>
        /// <param name="steeringParameters"></param>
        public override void InstantiateContextMap(PlanarSteeringParameters steeringParameters)
        {
            this.steeringParameters = steeringParameters;
            this.steeringParameters.OnResolutionChange += MapResolutionChangeHandler;
            steeringMap = new float[steeringParameters.ContextMapResolution];
            nextMap = new NativeArray<float>(steeringParameters.ContextMapResolution, Allocator.Persistent);
        }

        /// <summary>
        /// Runs when the context map resolution is changed at runtime
        /// </summary>
        private void MapResolutionChangeHandler()
        {
            steeringMap = new float[steeringParameters.ContextMapResolution];
        }

        /// <summary>
        /// Return a context map where the index of the float defines the direction whose value is being inspected, the size of the scalar defines how much we want to move in a direction
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
        public DotToVecJob GetJob()
        {
            Vector3[] targetArr = getPositionVectors();

            targetPositions = new NativeArray<Vector3>(targetArr.Length, Allocator.Persistent);

            for (int i = 0; i < targetArr.Length; i++)
            {
                targetPositions[i] = targetArr[i];
            }

            return new DotToVecJob()
            {
                targets = targetPositions,
                my_position = transform.position,
                range = Range,
                weight = Weight,
                angle = steeringParameters.ResolutionAngle,
                Weights = nextMap,
                direction = Direction,
                scaled = ScaleOnDistance,
                invertScale = invertScalef,
                axis = steeringParameters.ContextMapRotationAxis
            };
        }

        /// <summary>
        /// swaps the completed job data into the steering map, run this when the jobs are complete before scheduling the next job
        /// </summary>
        public void Swap()
        {
            targetPositions.Dispose();
            float[] next = new float[steeringParameters.ContextMapResolution];
            for (int i = 0; i < nextMap.Length; i++)
            {
                next[i] = nextMap[i];
            }

            steeringMap = next;
        }

        protected abstract Vector3[] getPositionVectors();


        protected virtual void OnDestroy()
        {
            steeringParameters.OnResolutionChange -= MapResolutionChangeHandler;
        }

        public virtual void OnDisable()
        {
            if (nextMap.IsCreated)
                nextMap.Dispose();
            if (targetPositions.IsCreated)
                targetPositions.Dispose();
        }

#if UNITY_EDITOR
        protected virtual void OnDrawGizmos()
        {

            if (steeringMap != null && MapDebugger != null)
            {
                MapVisualiserPlanar.InDrawGizmos(MapDebugger, steeringMap, Range, steeringParameters.ResolutionAngle, transform);
            }

        }
#endif

    }

}

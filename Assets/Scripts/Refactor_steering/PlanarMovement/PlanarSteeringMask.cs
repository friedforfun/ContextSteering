using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Friedforfun.SteeringBehaviours.Core;
using Friedforfun.SteeringBehaviours.Utilities;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Friedforfun.SteeringBehaviours.PlanarMovement
{
    public abstract class PlanarSteeringMask : CoreSteeringMask<PlanarSteeringParameters>
    {
        protected float[] maskMap; //The map of mask weights, each element represents our degree of interest in the direction that element corresponds to.
        protected float resolutionAngle; // Each point is seperated by a some degrees rotation (360/contextMap.Length)
        protected RotationAxis ContextMapAxis;

        [SerializeField] public MapVisualiserParameters MapDebugger;
        private MapVisualiser2D MapDebugVis = new MapVisualiser2D();


        /// <summary>
        /// Build a mask map where the index of the float defines the direction we wish to move, the size of the scalar defines how much we want to move in a direction
        /// </summary>
        /// <returns></returns>
        public abstract float[] BuildMaskMap();

        public override void InstantiateMaskMap(PlanarSteeringParameters steeringParameters)
        {
            ContextMapAxis = steeringParameters.ContextMapRotationAxis;
            var resolution = steeringParameters.ContextMapResolution;
            resolutionAngle = 360 / (float)resolution;
            maskMap = new float[resolution];
        }

        protected Quaternion rotateAroundAxis(float resolutionAngle)
        {
            return MapOperations.rotateAroundAxis(ContextMapAxis, resolutionAngle);
        }



#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            MapDebugVis.InDrawGizmos(MapDebugger, maskMap, Range, resolutionAngle, transform);
        }
#endif
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Friedforfun.ContextSteering.Core;

namespace Friedforfun.ContextSteering.CubicMovement
{
    public class CubicSteeringParameters : CoreSteeringParameters
    {
        [SerializeField]
        [Tooltip("The number of directions within the context sphere to evaluate.")]
        private int _contextSphereResolution = 48;

        public int ContextSphereResolution
        {
            get { return _contextSphereResolution; }
            set
            {
                _contextSphereResolution = value;
                updateResolutionAngle();
            }
        }

        private float _resolutionAngle;

        [HideInInspector]
        public float ResolutionAngle
        {
            get
            {
                if (_resolutionAngle == 0)
                    updateResolutionAngle();
                return _resolutionAngle;
            }
            private set
            {
                _resolutionAngle = value;
            }
        }

        private void updateResolutionAngle()
        {
            throw new System.NotImplementedException();
        }
    }
}

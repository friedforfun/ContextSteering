using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Friedforfun.SteeringBehaviours.Core;

namespace Friedforfun.SteeringBehaviours.PlanarMovement
{
    [System.Serializable]
    public class PlanarSteeringParameters: CoreSteeringParameters
    {

        [Tooltip("Number of directions the context map represents, higher values result in more overhead but allow more complex movement.")]
        [Range(4, 64)]
        private int m_contextMapResolution = 12;
        public int ContextMapResolution
        {
            get { return m_contextMapResolution; }
            set
            {
                m_contextMapResolution = value;
                ResolutionAngle = 360 / (float) m_contextMapResolution;
            }
        }


        [Tooltip("The axis that this steering module will compute the steering vectors around, usually Y axis for a 3D game and Z axis for a 2D game.")]
        public RotationAxis ContextMapRotationAxis;

        [Tooltip("The initial vector that all behaviours will evaluate first and begin rotating from.")]
        public Vector3 InitialVector;

        [HideInInspector]
        public float ResolutionAngle;
              

    }
}


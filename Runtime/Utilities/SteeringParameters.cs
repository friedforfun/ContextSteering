using UnityEngine;

namespace Friedforfun.ContextSteering.Utilities
{
    [System.Serializable]
    public class SteeringParameters
    {
        [Tooltip("Number of directions the context map represents, higher values result in more overhead but allow more complex movement.")]
        [Range(4, 64)] public int ContextMapResolution = 12;

        [Tooltip("The axis that this steering module will compute the steering vectors around, usually Y axis for a 3D game and Z axis for a 2D game.")]
        public RotationAxis ContextMapRotationAxis;

        [Tooltip("The initial vector that all behaviours will evaluate first and begin rotating from.")]
        public Vector3 InitialVector;
    }
}


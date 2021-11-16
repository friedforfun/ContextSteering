using UnityEngine;

namespace Friedforfun.ContextSteering.Core
{
    public abstract class CoreSteeringMask<T> : MonoBehaviour where T: CoreSteeringParameters
    {
        [Header("Mask Properties")]
        [Tooltip("Range at which the mask has an effect.")]
        [SerializeField] protected float Range = 10f;

        public string MaskName;

        /// <summary>
        /// Instantiate the initial steering mask data
        /// </summary>
        /// <param name="steeringParameters"></param>
        public abstract void InstantiateMaskMap(T steeringParameters);
    }
}


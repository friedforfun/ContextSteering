using UnityEngine;

namespace Friedforfun.SteeringBehaviours.Core
{
    public abstract class CoreSteeringMask<T> : MonoBehaviour where T: CoreSteeringParameters
    {
        [Tooltip("Range at which the mask has an effect.")]
        [SerializeField] protected float Range;

        /// <summary>
        /// Instantiate the initial steering mask data
        /// </summary>
        /// <param name="steeringParameters"></param>
        public abstract void InstantiateMaskMap(T steeringParameters);
    }
}


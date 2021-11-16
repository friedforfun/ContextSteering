using UnityEngine;
using Friedforfun.ContextSteering.Utilities;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Friedforfun.ContextSteering.Core
{
    /// <summary>
    /// All steering behaviours should inherit from this base class. And implement either ICreateSteeringJob or IDefineSteering
    /// </summary> 
    public abstract class CoreSteeringBehaviour<T> : MonoBehaviour where T: CoreSteeringParameters
    {
        [Header("Behaviour Properties")]
        [Tooltip("Range at which the behaviour has an effect.")]
        [SerializeField] protected float Range = 10f;
        
        protected Vector3 InitialVector;

        public string BehaviourName;
        public abstract void InstantiateContextMap(T steeringParameters);

    }
}



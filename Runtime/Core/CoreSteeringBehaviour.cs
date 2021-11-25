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

        [Tooltip("Is this behaviour attracted to this target or repulsed by it?")]
        [SerializeField] protected SteerDirection Direction = SteerDirection.ATTRACT;
        [Tooltip("How influential this behaviour is.")]
        [SerializeField] protected float Weight = 1f;
        [Tooltip("Does the behaviour scale its effect based on distance?")]
        [SerializeField] protected bool ScaleOnDistance = false;
        [Tooltip("If using scaling, set to true to make targets more important as they approach, false sets targets further away to be more significant.")]
        [SerializeField] protected bool InvertScale = true;
        protected float invertScalef { get { return InvertScale ? 1f : 0f; } }

        protected Vector3 InitialVector;

        public string BehaviourName;
        public abstract void InstantiateContextMap(T steeringParameters);

    }
}



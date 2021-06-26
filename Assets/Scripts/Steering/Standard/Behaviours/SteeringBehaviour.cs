using UnityEngine;
using Friedforfun.SteeringBehaviours.Utilities;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Friedforfun.SteeringBehaviours.Core2D
{
    /// <summary>
    /// All steering behaviours should inherit from this base class.
    /// </summary> 
    public abstract class SteeringBehaviour : MonoBehaviour
    {
        [Tooltip("Range at which the behaviour has an effect.")]
        [SerializeField] protected float Range;
        protected int resolution { get; private set; } // The number of directions we compute weights for.
        protected float[] steeringMap = null; // The map of weights, each element represents our degree of interest in the direction that element corresponds to.
        protected float resolutionAngle { get; private set; } // Each point is seperated by a some degrees rotation (360/steeringMap.Length)
        protected RotationAxis ContextMapAxis;
        protected Vector3 InitialVector;



        [Header("Debug")]
        [SerializeField] private bool ShowDebug = false;
        [SerializeField] private float MapSize = 2f;
        [SerializeField] private Color DebugColor = Color.green;

        /// <summary>
        /// Instantiates the context map weights and computes the angle between each direction
        /// </summary>
        /// <param name="resolution"></param>
        public void InstantiateContextMap(SteeringParameters steeringParameters)
        {
            InitialVector = steeringParameters.InitialVector;
            ContextMapAxis = steeringParameters.ContextMapRotationAxis;
            resolution = steeringParameters.ContextMapResolution;
            resolutionAngle = 360 / (float)resolution;
            steeringMap = new float[resolution];
        }

        /// <summary>
        /// Build a context map where the index of the float defines the direction we wish to move, the size of the scalar defines how much we want to move in a direction
        /// </summary>
        /// <returns></returns>
        public abstract float[] BuildContextMap();


        protected Quaternion rotateAroundAxis(float resolutionAngle)
        {
            return MapOperations.rotateAroundAxis(ContextMapAxis, resolutionAngle);
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {

            if (!ShowDebug || steeringMap is null || steeringMap.Length == 0)
            {
                return;
            }

            Vector3 position = transform.position;
            Handles.DrawWireDisc(position, Vector3.up, Range);

            position = new Vector3(position.x, position.y + 0.1f, position.z);
            Vector3 direction = Vector3.forward;

            foreach (float weight in MapOperations.NormaliseMap(steeringMap, MapSize))
            {
                Gizmos.color = DebugColor;
                Gizmos.DrawRay(transform.position, direction * weight);
                direction = Quaternion.Euler(0f, resolutionAngle, 0) * direction;
            }
            Handles.color = DebugColor;
            Handles.DrawWireDisc(position, Vector3.up, MapSize);

        }
#endif

    }
}


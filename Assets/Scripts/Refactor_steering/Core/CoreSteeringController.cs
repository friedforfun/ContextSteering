using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Friedforfun.SteeringBehaviours.Utilities;

namespace Friedforfun.SteeringBehaviours.Core
{
    /// <summary>
    /// Core steering behaviour controller, all behaviours are managed by this component.
    /// </summary>
    public class CoreSteeringController : MonoBehaviour
    {
        [Header("Behaviours")]
        public SteeringParameters steeringParameters;

        [Tooltip("Attractor and Repulsor strategies (directions in which we will move towards or away from.)")]
        public CoreSteeringBehaviour[] SteeringBehaviours;

        [Tooltip("Masking strategies (directions in which to block movement).")]
        public CoreSteeringMask[] SteeringMasks;

        [Tooltip("A strategy for combining steering and mask maps.")]
        protected ICombineContext ContextCombinator;

        [Tooltip("A strategy for selecting the final output direction from the context map.")]
        protected IDecideDirection DirectionDecider;

        private float[] contextMap; // The weights of each direction in the context map itself
        private Vector3 lastVector; // The previous vector output.

        /// <summary>
        /// Gets the most desired direction to move in. Normalised.
        /// </summary>
        /// <returns></returns>
        public Vector3 MoveDirection()
        {
            return lastVector.normalized;
        }

        /// <summary>
        /// Probably should not be used directly as a move input.
        /// Get the direction and magnitude (not normalised) of the most desired direction, use depends on behaviour implementations.
        /// For example if you want to know how "strongly" the agent wants to move in this direction
        /// </summary>
        /// <returns></returns>
        public Vector3 MoveVector()
        {
            return lastVector;
        }

        /// <summary>
        /// Merge bag of context maps by summing each element
        /// </summary>
        /// <param name="maps"></param>
        /// <returns></returns>
        protected float[] mergeMaps(ICollection<float[]> maps)
        {
            float[] contextMap = new float[steeringParameters.ContextMapResolution];
            for (int i = 0; i < steeringParameters.ContextMapResolution; i++)
            {
                contextMap[i] = 0f;
            }

            foreach (float[] map in maps)
            {
                var newMap = contextMap.Zip(map, (x, y) => x + y);
                contextMap = newMap.ToArray();
            }

            return contextMap;
        }

    }

}

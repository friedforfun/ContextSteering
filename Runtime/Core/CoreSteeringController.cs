using UnityEngine;

namespace Friedforfun.ContextSteering.Core
{
    /// <summary>
    /// Core steering behaviour controller, all behaviours are managed by this component.
    /// </summary>
    public class CoreSteeringController : MonoBehaviour
    {


        [Tooltip("A strategy for combining steering and mask maps.")]
        protected ICombineContext ContextCombinator;
        protected ICombineContext SetContextCombinator(ICombineContext newCombinator) 
        {
            ContextCombinator = newCombinator; return ContextCombinator; 
        }

        [Tooltip("A strategy for selecting the final output direction from the context map.")]
        protected IDecideDirection DirectionDecider;
        protected IDecideDirection SetDirectionDecider(IDecideDirection newDirectionDecider) 
        {
            DirectionDecider = newDirectionDecider; return DirectionDecider; 
        }

        protected Vector3 outputVector; // The vector output. Update this value when a new movement direction has been calculated.

        /// <summary>
        /// Gets the most desired direction to move in. Normalised.
        /// </summary>
        /// <returns></returns>
        public Vector3 MoveDirection()
        {
            return outputVector.normalized;
        }

        /// <summary>
        /// Probably should not be used directly as a move input.
        /// Get the direction and magnitude (not normalised) of the most desired direction, use depends on behaviour implementations.
        /// For example if you want to know how "strongly" the agent wants to move in this direction
        /// </summary>
        /// <returns></returns>
        public Vector3 MoveVector()
        {
            return outputVector;
        }


    }

}

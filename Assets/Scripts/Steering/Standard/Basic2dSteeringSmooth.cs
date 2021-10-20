using UnityEngine;
using Friedforfun.SteeringBehaviours.PlanarMovement;

namespace Friedforfun.SteeringBehaviours.Core2D
{
    ///<Summary>
    /// Basic simple steering with smoothing over direction changing, depending on the frequency that the direction is computed.
    ///</Summary>
    public class Basic2dSteeringSmooth : BaseContextSteering2D
    {
        [Range(-1, 1)]
        [SerializeField] float DotChangeThreshold = 0.3f;
        public Basic2dSteeringSmooth()
        {
            ContextCombinator = new BasicContextCombinator();
            DirectionDecider = new PlanarDirectionSimpleSmoothing(DotChangeThreshold);
        }
    }

}

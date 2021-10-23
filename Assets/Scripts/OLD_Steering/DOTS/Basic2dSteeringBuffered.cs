using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Friedforfun.SteeringBehaviours.PlanarMovement;

namespace Friedforfun.SteeringBehaviours.Core2D.Buffered
{
    public class Basic2dSteeringBuffered : BaseContextSteering2DBuffered
    {
        [SerializeField] bool AllowVectorZero = true;

        public Basic2dSteeringBuffered()
        {
            ContextCombinator = new BasicContextCombinator();
            DirectionDecider = new BasicPlanarDirectionPicker(AllowVectorZero, null);
        }
    }
}


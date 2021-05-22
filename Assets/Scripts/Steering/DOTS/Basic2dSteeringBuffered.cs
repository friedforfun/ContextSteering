using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Basic2dSteeringBuffered : BaseContextSteering2DBuffered
{
    [SerializeField] bool AllowVectorZero = true;

    public Basic2dSteeringBuffered()
    {
        ContextCombinator = new BasicContextCombinator();
        DirectionDecider = new BasicDirectionPicker(AllowVectorZero);
    }
}


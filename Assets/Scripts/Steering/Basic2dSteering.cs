using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Basic2dSteering : BaseContextSteering2D
{
    public Basic2dSteering()
    {
        ContextCombinator = new BasicContextCombinator();
        DirectionDecider = new DirectionSimpleSmoothing(0.3f);
    }
}

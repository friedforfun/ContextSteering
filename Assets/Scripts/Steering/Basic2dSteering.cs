using System.Collections;
using System.Collections.Generic;
using UnityEngine;


///<Summary>
/// Basic sample steering no smoothing over direction changing.
///</Summary>
public class Basic2dSteering : BaseContextSteering2D
{
    public Basic2dSteering()
    {
        ContextCombinator = new BasicContextCombinator();
        DirectionDecider = new BasicDirectionPicker();
    }
}

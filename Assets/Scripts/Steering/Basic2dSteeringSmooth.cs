using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Basic2dSteeringSmooth : BaseContextSteering2D
{
    [Range(-1, 1)]
    [SerializeField] float Smoothness = 0.3f;
    public Basic2dSteeringSmooth()
    {
        ContextCombinator = new BasicContextCombinator();
        DirectionDecider = new DirectionSimpleSmoothing(Smoothness);
    }
}

using System;
using UnityEngine;
using Friedforfun.ContextSteering.Core;


namespace Friedforfun.ContextSteering.PlanarMovement
{
    public class DotToTagMask : PlanarSteeringMask
    {
        [SerializeField]
        string[] Tags;

        protected override Vector3[] getPositionVectors()
        {
            var res = VectorsFromTagArray.GetVectors(Tags);
            return res;
        }
    }
}


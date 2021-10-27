using System;
using UnityEngine;
using Friedforfun.SteeringBehaviours.Core;


namespace Friedforfun.SteeringBehaviours.PlanarMovement
{
    public class DotToTagMask : PlanarSteeringMask
    {
        [SerializeField]
        string[] Tags;

        protected override Vector3[] getPositionVectors()
        {
            return VectorsFromTagArray.GetVectors(Tags);
        }
    }
}


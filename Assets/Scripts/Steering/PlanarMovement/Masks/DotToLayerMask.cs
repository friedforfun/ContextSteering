using UnityEngine;
using Friedforfun.SteeringBehaviours.Core;

namespace Friedforfun.SteeringBehaviours.PlanarMovement
{
    public class DotToLayerMask : PlanarSteeringMask
    {
        [SerializeField] public LayerMask Layers;

        protected override Vector3[] getPositionVectors()
        {
            return VectorsFromLayerMask.GetVectors(Layers, transform, Range);
        }
    }
}


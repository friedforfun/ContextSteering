using UnityEngine;
using Friedforfun.ContextSteering.Core;

namespace Friedforfun.ContextSteering.PlanarMovement
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


using UnityEngine;
using Friedforfun.ContextSteering.Core;

namespace Friedforfun.ContextSteering.PlanarMovement
{
    public class DotToTag : PlanarSteeringBehaviour
    { 
        [SerializeField] public string[] Tags;

        protected override Vector3[] getPositionVectors()
        {
            return VectorsFromTagArray.GetVectors(Tags);
        }

    }

}

using UnityEngine;
using Friedforfun.SteeringBehaviours.Core;

namespace Friedforfun.SteeringBehaviours.PlanarMovement
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

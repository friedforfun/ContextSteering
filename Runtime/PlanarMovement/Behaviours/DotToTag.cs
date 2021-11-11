using UnityEngine;
using Friedforfun.SteeringBehaviours.Core;

namespace Friedforfun.SteeringBehaviours.PlanarMovement
{
    public class DotToTag : PlanarSteeringBehaviour
    { 
        [SerializeField] public string[] Tags;

        protected override Vector3[] getPositionVectors()
        {
            if (Tags != null)
                return VectorsFromTagArray.GetVectors(Tags);

            return new Vector3[0];
        }

    }

}

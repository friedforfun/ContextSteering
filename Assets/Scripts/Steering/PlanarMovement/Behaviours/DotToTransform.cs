using Unity.Collections;
using UnityEngine;
using Friedforfun.SteeringBehaviours.Core;

namespace Friedforfun.SteeringBehaviours.PlanarMovement
{
    public class DotToTransform : PlanarSteeringBehaviour
    {
        [SerializeField] public Transform[] Positions;

        protected override Vector3[] getPositionVectors()
        {
            Vector3[] targets = new Vector3[Positions.Length];
            for (int i = 0; i < Positions.Length; i++)
            {
                targets[i] = Positions[i].position;
            }
            return targets;
        }

    }

}

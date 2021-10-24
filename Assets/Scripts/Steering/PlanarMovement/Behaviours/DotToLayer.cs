using Unity.Collections;
using UnityEngine;
using Friedforfun.SteeringBehaviours.Core;
using Friedforfun.SteeringBehaviours.Utilities;
using System.Collections.Generic;
using System;

namespace Friedforfun.SteeringBehaviours.PlanarMovement
{
    public class DotToLayer : PlanarSteeringBehaviour
    {

        [SerializeField] public LayerMask Layers;

        protected override Vector3[] getPositionVectors()
        {
            var collisionList = new List<Vector3>();
            Collider[] checkLayers = Physics.OverlapSphere(transform.position, Range, Layers);
            if (checkLayers != null)
            {
                foreach (Collider collision in checkLayers)
                {
                    collisionList.Add(collision.ClosestPoint(transform.position));
                }
            }

            return collisionList.ToArray();
        }

    }

}

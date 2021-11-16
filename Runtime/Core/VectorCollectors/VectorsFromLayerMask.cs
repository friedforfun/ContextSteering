using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Friedforfun.ContextSteering.Core
{
    public static class VectorsFromLayerMask 
    {
        public static Vector3[] GetVectors(LayerMask Layers, Transform transform, float Range)
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


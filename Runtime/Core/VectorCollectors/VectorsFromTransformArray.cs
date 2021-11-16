using UnityEngine;

namespace Friedforfun.ContextSteering.Core
{
    public static class VectorsFromTransformArray
    {
        public static Vector3[] GetVectors(Transform[] Positions)
        {
            if (Positions != null)
            {
                Vector3[] targets = new Vector3[Positions.Length];
                for (int i = 0; i < Positions.Length; i++)
                {
                    targets[i] = Positions[i].position;
                }
                return targets;
            }
            return new Vector3[0];

        }
    }
}


using System.Linq;
using Friedforfun.ContextSteering.Core;

namespace Friedforfun.ContextSteering.PlanarMovement
{
    /// <summary>
    /// Masks out all directions with more than the least danger + a threshold
    /// </summary>
    public class BasicContextCombinator : ICombineContext
    {
        private float dangerThreshold = 0.1f; // Allow steering towards a small degree of danger

        public float[] CombineContext(float[] steeringMap, float[] maskMap)
        {
            for (int i = 0; i < maskMap.Length; i++)
            {
                if (maskMap[i] < 0f)
                {
                    maskMap[i] = 0f;
                }
            }

            float[] contextMap = new float[steeringMap.Length];
            float lowestDanger = maskMap.Min();
            for (int i = 0; i < maskMap.Length; i++)
            {
                if (maskMap[i] > lowestDanger + dangerThreshold)
                {
                    contextMap[i] = 0;
                }
                else
                {
                    contextMap[i] = steeringMap[i];
                }
            }
            return contextMap;
        }
    }

}


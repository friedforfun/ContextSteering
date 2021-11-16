using System.Linq;
using Unity.Collections;
using UnityEngine;

namespace Friedforfun.ContextSteering.Utilities
{
    /// <summary>
    /// Convienience operations for context maps
    /// </summary>
    public static class MapOperations
    {

        /// <summary>
        /// Returns vector representing direction and magnitute from self to the target
        /// </summary>
        /// <param name="self"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static Vector3 VectorToTarget(GameObject self, GameObject other)
        {
            return other.transform.position - self.transform.position;
        }

        /// <summary>
        /// Returns vector representing direction and magnitute from self to the target
        /// </summary>
        /// <param name="self"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static Vector3 VectorToTarget(Vector3 self, Vector3 other)
        {
            return other - self;
        }

        /// <summary>
        /// Reverse a context maps magnitudes, so it points in the opposite directions as input
        /// </summary>
        /// <param name="contextMap"></param>
        /// <returns></returns>
        public static float[] ReverseMap(float[] contextMap)
        {
            int clen = contextMap.Length;
            int half_clen = clen / 2;

            float[] reverseMap = new float[contextMap.Length];
            for (int i = 0; i < contextMap.Length; i++)
            {


                if (i < half_clen)
                {
                    reverseMap[i + (half_clen)] = contextMap[i];
                }
                else if (i == half_clen)
                {
                    reverseMap[0] = contextMap[i];
                }
                else if (i > half_clen)
                {
                    reverseMap[i - (half_clen)] = contextMap[i];
                }
            }

            return reverseMap;
        }

        public static NativeArray<float> ReverseMap(NativeArray<float> contextMap)
        {
            int clen = contextMap.Length;
            int half_clen = clen / 2;

            NativeArray<float> reverseMap = new NativeArray<float>(clen, Allocator.Temp);
            for (int i = 0; i < clen; i++)
            {


                if (i < half_clen)
                {
                    reverseMap[i + (half_clen)] = contextMap[i];
                }
                else if (i == half_clen)
                {
                    reverseMap[0] = contextMap[i];
                }
                else if (i > half_clen)
                {
                    reverseMap[i - half_clen] = contextMap[i];
                }
            }

            for (int i = 0; i < clen; i++)
            {
                contextMap[i] = reverseMap[i];
            }
            reverseMap.Dispose();
            return contextMap;
        }

        /// <summary>
        /// Normalise context map between 0 and 1
        /// </summary>
        /// <param name="contextMap"></param>
        /// <returns></returns>
        public static float[] NormaliseMap(float[] contextMap)
        {
            float[] normMap = new float[contextMap.Length];
            float minVal = contextMap.Min();
            float maxVal = contextMap.Max();
            for (int i = 0; i < contextMap.Length; i++)
            {
                normMap[i] = (contextMap[i] - minVal) / (maxVal - minVal);
            }
            return normMap;
        }



        /// <summary>
        /// Normalise context map between 0 and max
        /// </summary>
        /// <param name="contextMap"></param>
        /// <returns></returns>
        public static float[] NormaliseMap(float[] contextMap, float max)
        {
            float[] normMap = new float[contextMap.Length];
            float minVal = contextMap.Min();
            float maxVal = contextMap.Max();
            for (int i = 0; i < contextMap.Length; i++)
            {
                normMap[i] = ((contextMap[i] - minVal) / (maxVal - minVal)) * max;
            }
            return normMap;
        }

        /// <summary>
        /// Normalise context map between min and max
        /// </summary>
        /// <param name="contextMap"></param>
        /// <returns></returns>
        public static float[] NormaliseMap(float[] contextMap, float min, float max)
        {
            float[] normMap = new float[contextMap.Length];
            float minVal = contextMap.Min();
            float maxVal = contextMap.Max();
            for (int i = 0; i < contextMap.Length; i++)
            {
                normMap[i] = ((contextMap[i] - minVal) / (maxVal - minVal)) * (max-min) + min;
            }
            return normMap;
        }

        public static Quaternion RotateAroundAxis(RotationAxis ContextMapAxis, float resolutionAngle)
        {
            switch (ContextMapAxis)
            {
                case RotationAxis.XAxis:
                    return Quaternion.Euler(resolutionAngle, 0f, 0f);

                case RotationAxis.YAxis:
                    return Quaternion.Euler(0f, resolutionAngle, 0f);

                case RotationAxis.ZAxis:
                    return Quaternion.Euler(0f, 0f, resolutionAngle);
            }

            throw new System.NotImplementedException("Rotation Axis undefined.");
        }
    }

}

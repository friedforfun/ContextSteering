using System.Linq;
using UnityEngine;

public static class MapOperations
{

    /// <summary>
    /// Returns vector representing direction and magnitute from self to the target
    /// </summary>
    /// <param name="self"></param>
    /// <param name="target"></param>
    /// <returns></returns>
    public static Vector3 VectorToTarget(GameObject self, GameObject target)
    {
        return target.transform.position - self.transform.position;
    }

    /// <summary>
    /// Returns vector representing direction and magnitute from self to the target
    /// </summary>
    /// <param name="self"></param>
    /// <param name="target"></param>
    /// <returns></returns>
    public static Vector3 VectorToTarget(Vector3 self, Vector3 target)
    {
        return target - self;
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
            normMap[i] = ((contextMap[i] - minVal) / (maxVal - minVal)) * max + min;
        }
        return normMap;
    }
}

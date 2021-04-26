using System.Linq;

public static class MapOperations
{
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
    /// Normalise context map between highest and lowext value in map
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
}

using System.Collections.Generic;

namespace Friedforfun.ContextSteering.Demo
{
    public static class Shuffle<T>
    {
        private static System.Random _random = new System.Random();

        public static List<T> Fisher_Yates_CardDeck_Shuffle(List<T> aList)
        {
            T ObjectInList;

            int n = aList.Count;
            for (int i = 0; i < n; i++)
            {
                int r = i + (int)(_random.NextDouble() * (n - i));
                ObjectInList = aList[r];
                aList[r] = aList[i];
                aList[i] = ObjectInList;
            }

            return aList;
        }
    }

}

using System.Collections.Generic;

namespace Friedforfun.SteeringBehaviours.Utilities
{
    public static class Shuffle<T>
    {
        public static List<T> Fisher_Yates_CardDeck_Shuffle(List<T> aList)
        {

            System.Random _random = new System.Random();

            T ObjectInList;

            int n = aList.Count;
            for (int i = 0; i < n; i++)
            {
                // NextDouble returns a random number between 0 and 1.
                // ... It is equivalent to Math.random() in Java.
                int r = i + (int)(_random.NextDouble() * (n - i));
                ObjectInList = aList[r];
                aList[r] = aList[i];
                aList[i] = ObjectInList;
            }

            return aList;
        }
    }

}

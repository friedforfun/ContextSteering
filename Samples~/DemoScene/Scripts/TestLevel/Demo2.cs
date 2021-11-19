using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace Friedforfun.ContextSteering.Demo
{
    public class Demo2 : MonoBehaviour
    {
        [SerializeField] private Spawner spawner;
    
        private GameObject[] targetPlates;

        // List of avaliable indices in targetPlates
        private List<int> indexPool;

        private void Awake()
        {
            targetPlates = GameObject.FindGameObjectsWithTag("TargetPlate");
            indexPool = Enumerable.Range(0, targetPlates.Length).ToList();
            indexPool = Shuffle<int>.Fisher_Yates_CardDeck_Shuffle(indexPool);
            spawner.Spawn();
        }

        /// <summary>
        /// Gets the next avaliable target plate
        /// </summary>
        /// <returns></returns>
        public GameObject GetTarget()
        {
            if (indexPool.Count > 0)
            {
                int index = indexPool[0];
                indexPool.RemoveAt(0);
                return targetPlates[index];
            }
            else
                return null;

        }

        /// <summary>
        /// Returns the target plate to pool and adds 
        /// </summary>
        /// <param name="go"></param>
        public void ReturnToPool(GameObject go)
        {
            int index = targetPlates.ToList().IndexOf(go);
            indexPool.Add(index);
            indexPool = Shuffle<int>.Fisher_Yates_CardDeck_Shuffle(indexPool);
        }

    }

}

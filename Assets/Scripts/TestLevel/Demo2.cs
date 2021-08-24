using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Friedforfun.SteeringBehaviours.Core2D.Buffered;

namespace Friedforfun.SteeringBehaviours.Demo
{
    public class Demo2 : MonoBehaviour
    {
        [SerializeField] private Spawner spawner;
    
        private GameObject[] targetPlates;
        private List<int> indexPool;

        private void Awake()
        {
            targetPlates = GameObject.FindGameObjectsWithTag("TargetPlate");
            indexPool = Enumerable.Range(0, targetPlates.Length).ToList();
            indexPool = Shuffle<int>.Fisher_Yates_CardDeck_Shuffle(indexPool);
            spawner.Spawn();

            StartCoroutine(rebuildDelay());
        }


        private IEnumerator rebuildDelay()
        {
            yield return new WaitForSeconds(2f);
            SteeringScheduler.RepopulateSteerers();
        }

        public GameObject GetTarget()
        {
            int index = indexPool[0];
            indexPool.RemoveAt(0);
            return targetPlates[index];
        }

        public void ReturnToPool(GameObject go)
        {
            int index = targetPlates.ToList().IndexOf(go);
            indexPool.Add(index);
            indexPool = Shuffle<int>.Fisher_Yates_CardDeck_Shuffle(indexPool);
        }

    }

}

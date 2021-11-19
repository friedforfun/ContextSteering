using UnityEngine;
using UnityEngine.Events;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Friedforfun.ContextSteering.Demo
{
    public class Spawner : MonoBehaviour
    {

        [SerializeField] private int spawnCount;
        [Range(1, 100)]
        [SerializeField] private int spawnAreaSize = 1;
        [SerializeField] private GameObject minion;
        [SerializeField] private LayerMask ignoreLayers;


        [SerializeField] private float minionOffset;
        private void Start()
        {
        
            if (minionOffset is 0f)
                minionOffset = transform.position.y;

        }

        public void Spawn()
        {
            for (int i = 0; i < spawnCount; i++)
            {
                Vector3 spawnPosition = GetSpawnPosition();

                Quaternion spawnRotation = new Quaternion();
                spawnRotation.eulerAngles = new Vector3(0.0f, Random.Range(0.0f, 360.0f));
                if (spawnPosition != Vector3.zero)
                {
                    Instantiate(minion, spawnPosition, spawnRotation);
                }
            }
        }

        Vector3 GetSpawnPosition()
        {
            Vector3 spawnPosition = transform.position;
            float startTime = Time.realtimeSinceStartup;
            bool test = false;
            while (test == false)
            {
                Vector2 spawnPositionRaw = Random.insideUnitCircle * spawnAreaSize;
                spawnPosition = new Vector3(spawnPosition.x + spawnPositionRaw.x, minionOffset, spawnPosition.z + spawnPositionRaw.y);
                test = !Physics.CheckSphere(spawnPosition, 0.75f, ignoreLayers);
                if (Time.realtimeSinceStartup - startTime > 0.5f)
                {
                    Debug.Log("Time out placing Minion!");
                    return Vector3.zero;
                }
            }
            return spawnPosition;
        }

    #if UNITY_EDITOR
        private void OnDrawGizmos()
        {

            Handles.color = Color.red;
            Handles.DrawWireDisc(transform.position, transform.up, spawnAreaSize);
        }
    #endif
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Friedforfun.ContextSteering.Utilities;

namespace Friedforfun.ContextSteering.Demo
{
    public class BouncingBall : MonoBehaviour
    {
        [SerializeField]
        Rigidbody rBody;

        [SerializeField]
        float impulseCoefficient = 1f;

        private void Start()
        {
            StartCoroutine(randomForces());
        }

        private void AddForce()
        {
            Vector3 direction = new Vector3(Random.Range(-180f, 180f), 0, Random.Range(-180f, 180f)).normalized;
            rBody.AddForce(direction * impulseCoefficient, ForceMode.Impulse);
        }


        private void Awake()
        {
            TagCache.Register(gameObject);
        }


        private void OnDisable()
        {
            TagCache.DeRegister(gameObject);
        }

        private IEnumerator randomForces()
        {
            for (; ; )
            {
                AddForce();
                yield return new WaitForSeconds(Random.Range(1f, 15f));
            }

        }

    }

}

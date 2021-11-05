using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Friedforfun.SteeringBehaviours.Utilities;

namespace Friedforfun.SteeringBehaviours.Demo
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
            ReferencePool.Register(gameObject);
        }


        private void OnDisable()
        {
            ReferencePool.DeRegister(gameObject);
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

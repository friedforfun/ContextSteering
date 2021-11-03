using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Friedforfun.SteeringBehaviours.Demo;

namespace Friedforfun.SteeringBehaviours.Demo
{
    public class TargetPlate : MonoBehaviour
    {
        private float maxStay = 2f;

        private void OnTriggerEnter(Collider other)
        {
            StartCoroutine(startCountdown(other));
        }



        private void FindNewTarget(Collider otherCol)
        {
            Demo2TargetSelector swarmdemo = otherCol.GetComponent<Demo2TargetSelector>();

            if (swarmdemo != null)
            {
                swarmdemo.ReaquireTarget(gameObject);
            }
        }

        private IEnumerator startCountdown(Collider otherCol)
        {
            yield return new WaitForSeconds(Random.Range(0.1f, maxStay));
            FindNewTarget(otherCol);
        }
    }
}


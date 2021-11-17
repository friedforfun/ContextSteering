using System.Collections;
using UnityEngine;

namespace Friedforfun.ContextSteering.Demo
{
    public class TargetPlate : MonoBehaviour
    {
        private float maxStay = 2f;

        private void OnTriggerEnter(Collider other)
        {
            StartCoroutine(startCountdown(other));
        }



        private void ClearTarget(Collider otherCol)
        {
            Demo2TargetSelector swarmdemo = otherCol.GetComponent<Demo2TargetSelector>();

            if (swarmdemo != null)
            {
                swarmdemo.ClearTargetFromBehaviour(gameObject);
            }
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
            ClearTarget(otherCol);
            yield return new WaitForSeconds(Random.Range(0.1f, maxStay));
            FindNewTarget(otherCol);
        }
    }
}


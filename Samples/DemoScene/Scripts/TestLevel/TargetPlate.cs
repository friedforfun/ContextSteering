using System.Collections;
using System.Linq;
using UnityEngine;
using Friedforfun.SteeringBehaviours.Demo;

namespace Friedforfun.SteeringBehaviours.Demo
{
    public class TargetPlate : MonoBehaviour
    {
        private string DemoID = null;
        private DemoCollisionTracker dct;
        private float maxStay = 2f;

        private void OnTriggerEnter(Collider other)
        {
            if (DemoID == null)
            {
                DemoID = other.gameObject.GetComponent<AgentCommon>()?.DemoID;
                dct = FindObjectsOfType<DemoCollisionTracker>().Where(colTracker => colTracker.DemoID == DemoID).First();
            }

            dct?.GoalAchieved();

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


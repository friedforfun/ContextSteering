using UnityEngine;
using System.Linq;
using Friedforfun.ContextSteering.Utilities;

namespace Friedforfun.ContextSteering.Demo
{
    public class BoxPorter : MonoBehaviour
    {
        [SerializeField] private Vector3[] jumpPoints;

        private string DemoID = null;
        private DemoCollisionTracker dct;
        private int jumpIndex = 0;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Agent"))
            {
                if (DemoID == null)
                {
                    DemoID = other.gameObject.GetComponent<AgentCommon>()?.DemoID;
                    dct = FindObjectsOfType<DemoCollisionTracker>().Where(colTracker => colTracker.DemoID == DemoID).First();
                }

                dct?.GoalAchieved();


                if (jumpIndex == jumpPoints.Length)
                {
                    jumpIndex = 0;
                }
                transform.localPosition = jumpPoints[jumpIndex];
                jumpIndex++;
            }
        }

        private void Awake()
        {
            //Debug.Log($"Registered Box under tag: {gameObject.tag}, at position: {transform.position}");

            TagCache.Register(gameObject);
        }


        private void OnDisable()
        {
            TagCache.DeRegister(gameObject);
        }

    }

}

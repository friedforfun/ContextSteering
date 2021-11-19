using System.Collections;
using System.Linq;
using UnityEngine;
using Friedforfun.ContextSteering.Utilities;

namespace Friedforfun.ContextSteering.Demo
{
    /// <summary>
    /// Common functionality between demo agents, registering to tag cache, and handling collision indicators.
    /// </summary>
    [SelectionBase]
    public class AgentCommon : MonoBehaviour
    {

        [SerializeField] private Renderer childRenderer;
        [SerializeField] private Material impactMaterial;
        [SerializeField] public string DemoID = null;

        public bool debug = false;

        private Material baseMaterial;
        private bool blockCollision = true;
        private DemoCollisionTracker dct;

        private void Start()
        {
            if (DemoID != null)
            {
                var dctArr = FindObjectsOfType<DemoCollisionTracker>();
                dct = dctArr.Where(colTracker => colTracker.DemoID == DemoID).First();
            }
  



            baseMaterial = childRenderer.material;
            StartCoroutine(collisionDelay());
        }

        private void Awake()
        {
            TagCache.Register(gameObject);
        }

        private void OnDisable()
        {
            TagCache.DeRegister(gameObject);
        }


        private void OnCollisionEnter(Collision collision)
        {
            if (blockCollision)
                return;

            blockCollision = true;
            StartCoroutine(collisionDelay());

            if (collision.gameObject.tag != "Floor" && collision.gameObject.tag != "TargetPlate")
            {
                if (debug)
                    Debug.Log($"Collided with: {collision.gameObject.name}");
                childRenderer.material = impactMaterial;
                if (dct != null)
                {
                    dct.CollisionOccured();
                }
            }

        }

        public void IncrementGoal()
        {
            dct.GoalAchieved();
        }

        private void OnCollisionExit(Collision collision)
        {
            StartCoroutine(resetColour());
        }

        IEnumerator resetColour()
        {
            yield return new WaitForSeconds(0.5f);
            childRenderer.material = baseMaterial;
        }

        IEnumerator collisionDelay()
        {
            yield return new WaitForSeconds(0.5f);
            blockCollision = false;
        }

    }

}

using System.Collections;
using UnityEngine;
using Friedforfun.SteeringBehaviours.Utilities;
using Friedforfun.SteeringBehaviours.Core2D.Buffered;

namespace Friedforfun.SteeringBehaviours.Demo
{
    /// <summary>
    /// Common functionality between demo agents, registering to tag cache, and handling collision indicators.
    /// </summary>
    [SelectionBase]
    public class AgentCommon : MonoBehaviour
    {

        [SerializeField] private Renderer childRenderer;
        [SerializeField] private Material impactMaterial;

        private Material baseMaterial;
        private bool blockCollision = true;

        private void Start()
        {
            baseMaterial = childRenderer.material;
            StartCoroutine(collisionDelay());
        }

        private void Awake()
        {
            ReferencePool.Register(gameObject);
        }

        private void OnDisable()
        {
            ReferencePool.DeRegister(gameObject);
        }


        private void OnCollisionEnter(Collision collision)
        {
            if (blockCollision)
                return;

            if (collision.gameObject.tag != "Floor")
            {
                childRenderer.material = impactMaterial;
                //Debug.Log($"Collided with: {collision.gameObject.name}");
            }

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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        TagRegistry.Register(gameObject);
    }

    private void OnDisable()
    {
        TagRegistry.DeRegister(gameObject);
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

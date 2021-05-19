using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwarmDemoBuffered : MonoBehaviour
{
    [SerializeField] private BaseContextSteering2DBuffered steer;
    [SerializeField] private CharacterController control;
    [SerializeField] private GameObject target;


    [SerializeField] private Renderer childRenderer;
    [SerializeField] private Material impactMaterial;
    private Material baseMaterial;

    private SteeringScheduler steeringScheduler;

    [Range(0.1f, 20f)]
    [SerializeField] private float Speed = 1f;
    private Vector3 LastDirection = Vector3.forward;
    private bool blockCollision = true;

    private void Start()
    {
        steeringScheduler = FindObjectOfType<SteeringScheduler>();

        baseMaterial = childRenderer.material;
        StartCoroutine(collisionDelay());

        steeringScheduler.duringContextUpdate += SchedulerUpdate;
    }

    private void OnDisable()
    {
        steeringScheduler.duringContextUpdate -= SchedulerUpdate;
    }

    void Update()
    {
        // Get the movement direction from the steering module
        LastDirection = steer.MoveDirection();

        // Apply movement based on direction obtained
        control.SimpleMove(LastDirection * Speed);

        if (target != null)
            // Look towards target
            transform.rotation = Quaternion.LookRotation(MapOperations.VectorToTarget(gameObject, target).normalized);
    }

    public void SchedulerUpdate()
    {
        // Get the movement direction from the steering module
        LastDirection = steer.MoveDirection();

        // Apply movement based on direction obtained
        control.SimpleMove(LastDirection * Speed);
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

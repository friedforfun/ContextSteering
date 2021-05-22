using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwarmDemoBuffered : MonoBehaviour
{
    [SerializeField] private BaseContextSteering2DBuffered steer;
    [SerializeField] private CharacterController control;
    [SerializeField] private GameObject target;


    private SteeringScheduler steeringScheduler;

    [Range(0.1f, 20f)]
    [SerializeField] private float Speed = 1f;
    private Vector3 LastDirection = Vector3.forward;

    private bool allowDirectionChange = true;
    private void Start()
    {
        steeringScheduler = FindObjectOfType<SteeringScheduler>();

        steeringScheduler.duringContextUpdate += SchedulerUpdate;
    }

    private void OnDisable()
    {
        steeringScheduler.duringContextUpdate -= SchedulerUpdate;
    }

    void Update()
    {
        // Apply movement based on direction obtained
        control.SimpleMove(LastDirection * Speed);

        if (target != null)
            // Look towards target
            transform.rotation = Quaternion.LookRotation(MapOperations.VectorToTarget(gameObject, target).normalized);
    }

    private void FixedUpdate()
    {
        // Get the movement direction from the steering module
        if (allowDirectionChange)
        {
            allowDirectionChange = false;
            LastDirection = steer.MoveDirection();
            StartCoroutine(resetDirectionBlocker());
        }

    }


    IEnumerator resetDirectionBlocker()
    {
        yield return new WaitForSeconds(0.1f);
        allowDirectionChange = true;
    }

    public void SchedulerUpdate()
    {
        // Get the movement direction from the steering module
        LastDirection = steer.MoveDirection();

        // Apply movement based on direction obtained
        control.SimpleMove(LastDirection * Speed);
    }





}

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





}

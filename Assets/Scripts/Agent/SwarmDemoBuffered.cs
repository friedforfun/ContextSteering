using Friedforfun.SteeringBehaviours.Utilities;
using UnityEngine;
using Friedforfun.SteeringBehaviours.Core2D.Buffered;

public class SwarmDemoBuffered : MonoBehaviour
{
    [SerializeField] private BaseContextSteering2DBuffered steer;
    [SerializeField] private CharacterController control;
    [SerializeField] private GameObject target;


    private SteeringScheduler steeringScheduler;

    [Range(0.1f, 20f)]
    [SerializeField] private float Speed = 1f;
    private Vector3 LastDirection = Vector3.forward;

    private Demo2 demoController;
    private BufferedDotToPosition dotToPosition;

    private void Start()
    {
        steeringScheduler = FindObjectOfType<SteeringScheduler>();

        //steeringScheduler.duringContextUpdate += SchedulerUpdate;

        dotToPosition = gameObject.GetComponent<BufferedDotToPosition>();

        demoController = FindObjectOfType<Demo2>();

        target = demoController.GetTarget();
        Transform[] positions = new Transform[1];
        positions[0] = target.transform;
        dotToPosition.UpdatePositions(positions);
    }

    public void ReaquireTarget(GameObject caller)
    {
        if (caller.Equals(target))
        {
            GameObject tempTarget = demoController.GetTarget();

            demoController.ReturnToPool(target);

            target = tempTarget;

            Transform[] positions = new Transform[1];
            positions[0] = target.transform;

            dotToPosition.UpdatePositions(positions);
        }
    }

    private void OnDisable()
    {
        //steeringScheduler.duringContextUpdate -= SchedulerUpdate;
    }

    void Update()
    {
        LastDirection = steer.MoveDirection();
        // Apply movement based on direction obtained
        control.SimpleMove(LastDirection * Speed);

        if (target != null)
        {
            // Look towards target

            Vector3 toTarget = MapOperations.VectorToTarget(gameObject, target);
            toTarget.y = 0f;
            transform.rotation = Quaternion.LookRotation(toTarget);
        }
            
    }


    public void SchedulerUpdate()
    {
        // Get the movement direction from the steering module
        LastDirection = steer.MoveDirection();

        // Apply movement based on direction obtained
        control.SimpleMove(LastDirection * Speed);
    }





}

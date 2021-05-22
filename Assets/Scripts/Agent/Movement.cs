using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class Movement : MonoBehaviour
{
    [SerializeField] private BaseContextSteering2D steer;
    [SerializeField] private CharacterController control;
    [SerializeField] private GameObject LookTarget;

    [Range(0.1f, 20f)]
    [SerializeField] private float Speed = 1f;
    private Vector3 LastDirection = Vector3.forward;

    private bool allowDirectionChange = true;

    void Update()
    {

        // Apply movement based on direction obtained
        control.SimpleMove(LastDirection * Speed);

        // Look towards target
        transform.rotation = Quaternion.LookRotation(MapOperations.VectorToTarget(gameObject, LookTarget).normalized);
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

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {

        Handles.color = Color.magenta;
        Handles.ArrowHandleCap(0, transform.position, Quaternion.LookRotation(LastDirection, Vector3.up), 2f, EventType.Repaint);
    }
#endif
}

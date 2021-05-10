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
    [SerializeField] private Renderer childRenderer;
    
    [Range(0.1f, 20f)]
    [SerializeField] private float Speed = 1f;
    private Vector3 LastDirection = Vector3.zero;

    private Color baseSpecColour;

    private void Start()
    {
        baseSpecColour = childRenderer.material.GetColor("_SpecColor");
    }

    void Update()
    {
        // Get the movement direction from the steering module
        LastDirection = steer.MoveDirection();

        // Apply movement based on direction obtained
        control.SimpleMove(LastDirection * Speed);
        
        // Look towards target
        transform.rotation = Quaternion.LookRotation(MapOperations.VectorToTarget(gameObject, LookTarget).normalized);
    }


    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Contact");
        childRenderer.material.SetColor("_SpecColor", Color.red);
    }

    private void OnCollisionExit(Collision collision)
    {
        StartCoroutine(resetColour());
    }

    IEnumerator resetColour()
    {
        yield return new WaitForSeconds(0.5f);
        childRenderer.material.SetColor("_SpecColor", baseSpecColour);
    }


#if UNITY_EDITOR
    private void OnDrawGizmos()
    {

        Handles.color = Color.magenta;
        Handles.ArrowHandleCap(0, transform.position, Quaternion.LookRotation(LastDirection, Vector3.up), 2f, EventType.Repaint);
    }
#endif
}

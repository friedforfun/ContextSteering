using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class Movement : MonoBehaviour
{
    [SerializeField] private BaseContextSteering2D steer;

    private Vector3 LastDirection = Vector3.zero;

    void Update()
    {
        LastDirection = steer.MoveDirection();
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {

        Handles.color = Color.magenta;
        Handles.ArrowHandleCap(0, transform.position, Quaternion.LookRotation(LastDirection, Vector3.up), 2f, EventType.Repaint);
    }
#endif
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public enum BuildContextMapOn
{
    UPDATE,
    FIXED_UPDATE,
    TIME
}

public class Steering2D : MonoBehaviour
{
    [Header("Behaviours")]
    public float Radius;
    public ISteeringBehaviour[] SteeringBehaviours;
    public ISteeringMask[] SteeringMasks;
    public BuildContextMapOn BuildMapOn;

    private float TimeInterval;

    private void Update()
    {
        if (BuildMapOn != BuildContextMapOn.UPDATE)
        {
            return;
        }
    }

    private void FixedUpdate()
    {
        if (BuildMapOn != BuildContextMapOn.FIXED_UPDATE)
        {
            return;
        }
    }


#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Vector3 position = transform.position;


        Handles.DrawWireDisc(position, Vector3.up, Radius);
    }
#endif
}

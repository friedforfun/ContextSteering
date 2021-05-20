using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YoYoMoveLerp : MonoBehaviour
{
    public Vector3 StartPos;
    public Vector3 EndPos;
    public float frequency = 1;

    const float TAU = 2 * Mathf.PI;
    private float lerpVal;

    private void Awake()
    {
        TagRegistry.Register(gameObject);
    }


    private void OnDisable()
    {
        TagRegistry.DeRegister(gameObject);
    }

    void Update()
    {
        lerpVal = 0.5f * (1 + Mathf.Sin(TAU * frequency * Time.time));
        transform.localPosition = Vector3.Lerp(StartPos, EndPos, lerpVal);
    }
}

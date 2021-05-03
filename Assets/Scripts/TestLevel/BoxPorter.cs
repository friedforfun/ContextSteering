using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxPorter : MonoBehaviour
{
    [SerializeField] private Vector3[] jumpPoints;
    private int jumpIndex = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Agent"))
        {
            if (jumpIndex == jumpPoints.Length)
            {
                jumpIndex = 0;
            }
            transform.localPosition = jumpPoints[jumpIndex];
            jumpIndex++;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetPlate : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        SwarmDemoBuffered swarmdemo = other.GetComponent<SwarmDemoBuffered>();

        if (swarmdemo != null)
        {
            swarmdemo.ReaquireTarget(gameObject);
        }
    }
}

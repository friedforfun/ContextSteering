using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Friedforfun.SteeringBehaviours.Demo;

namespace Friedforfun.SteeringBehaviours.Demo
{
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
}


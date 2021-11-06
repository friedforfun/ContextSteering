using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Friedforfun.SteeringBehaviours.PlanarMovement;

namespace Friedforfun.SteeringBehaviours.Demo
{
    public class Demo3NavmeshBuilder : MonoBehaviour
    {
        [SerializeField] Transform Target;

        private DotToNavmeshPath behaviour;

        private void Start()
        {
            behaviour = GetComponent<DotToNavmeshPath>();

            RecomputePath();
        }

        public void RecomputePath()
        {
            if (!behaviour.CalculatePath(Target.transform.position))
            {
                Debug.Log("Path Failed to build");
            }
        }

    }
}

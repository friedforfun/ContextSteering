using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Friedforfun.SteeringBehaviours.PlanarMovement;
using Friedforfun.SteeringBehaviours.Utilities;

namespace Friedforfun.SteeringBehaviours.Demo
{
    public class Demo3NavmeshBuilder : MonoBehaviour
    {
        [SerializeField] Transform Target;

        private DotToNavmeshPath behaviour;

        private float recomputePathThreshold = 2f;
        private Vector3 lastPosition;

        private void Start()
        {
            behaviour = GetComponent<DotToNavmeshPath>();
            lastPosition = transform.position;
            RecomputePath();
            StartCoroutine(StuckCheck());

        }


        public void RecomputePath()
        {
            if (!behaviour.CalculatePath(Target.transform.position))
            {
                Debug.Log("Path Failed to build");
            }
        }

        private IEnumerator StuckCheck()
        {
            for (; ; )
            {
                yield return new WaitForSeconds(1);
                if (MapOperations.VectorToTarget(transform.position, lastPosition).magnitude < recomputePathThreshold)
                {
                    //Debug.Log("Recomputing Path");
                    RecomputePath();
                }
                lastPosition = transform.position;
                    
            }

        }

    }
}

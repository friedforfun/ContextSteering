using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Friedforfun.ContextSteering.PlanarMovement;
using Friedforfun.ContextSteering.Utilities;

namespace Friedforfun.ContextSteering.Demo
{
    public class Demo3NavmeshBuilder : MonoBehaviour
    {
        [SerializeField] Transform Target;

        private DotToNavmeshPath behaviour;
        private AgentCommon agentCommon;

        private float recomputePathThreshold = 2f;
        private Vector3 lastPosition;

        private void Start()
        {
            behaviour = GetComponent<DotToNavmeshPath>();
            agentCommon = GetComponent<AgentCommon>();
            lastPosition = transform.position;
            RecomputePath();
            StartCoroutine(StuckCheck());

        }

        public void HitGoal()
        {
            agentCommon.IncrementGoal();
            RecomputePath();
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

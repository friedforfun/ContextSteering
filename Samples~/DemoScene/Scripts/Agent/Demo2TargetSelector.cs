using System.Collections;
using System.Linq;
using UnityEngine;
using Friedforfun.ContextSteering.PlanarMovement;

namespace Friedforfun.ContextSteering.Demo
{
    public class Demo2TargetSelector : MonoBehaviour
    {
        [SerializeField]
        private DotToTransform transformBehaviour;

        [SerializeField]
        private PlanarMovement mov;

        public GameObject target;
        private Demo2 demoController;
        private DemoCollisionTracker dct;


        void Start()
        {
            demoController = FindObjectOfType<Demo2>();
            target = demoController.GetTarget();
            if (target != null)
            {
                Transform[] positions = { target.transform };
                transformBehaviour.Positions = positions;
                mov.LookTarget = target;
            }


            AgentCommon ac = null;

            if (!TryGetComponent(out ac))
            {
                ac = gameObject.GetComponentInParent<AgentCommon>();
            }

            if (ac == null)
            {
                ac = gameObject.GetComponentInChildren<AgentCommon>();
            }

            if (ac != null)
            {
                dct = FindObjectsOfType<DemoCollisionTracker>().Where(colTracker => colTracker.DemoID == ac.DemoID).First();
            }
            else
            {
                Debug.LogWarning("Could not find a valid AgentCommon Component in colliding game object.");
            }
            


        }

        /// <summary>
        /// Clears the target from behaviour moving this object towards target.
        /// </summary>
        /// <param name="caller"></param>
        public void ClearTargetFromBehaviour(GameObject caller)
        {
            if (caller.Equals(target))
            {
                transformBehaviour.Positions = null;
            }
        }
        
        /// <summary>
        /// Collision with a target plate will call this function
        /// </summary>
        /// <param name="caller"></param>
        public void ReaquireTarget(GameObject caller)
        {
            if (caller.Equals(target))
            {
                GameObject tempTarget = demoController.GetTarget();
                demoController.ReturnToPool(target);

                if (tempTarget != null)
                {
                    target = tempTarget;

                    Transform[] positions = { target.transform };

                    transformBehaviour.Positions = positions;
                    mov.LookTarget = target;
                }

                dct?.GoalAchieved();

            }
        }
    }

}

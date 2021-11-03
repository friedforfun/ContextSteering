using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Friedforfun.SteeringBehaviours.PlanarMovement;

namespace Friedforfun.SteeringBehaviours.Demo
{
    public class Demo2TargetSelector : MonoBehaviour
    {
        [SerializeField]
        private DotToTransform transformBehaviour;

        [SerializeField]
        private PlanarMovement mov;

        private GameObject target;
        private Demo2 demoController;
        

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

            }
        }
    }

}

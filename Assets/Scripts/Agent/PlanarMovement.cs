using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Friedforfun.SteeringBehaviours.PlanarMovement;
using Friedforfun.SteeringBehaviours.Utilities;
using UnityEditor;

namespace Friedforfun.SteeringBehaviours.Demo
{
    public class PlanarMovement : MonoBehaviour
    {
        [SerializeField] private PlanarSteeringController steer;
        [SerializeField] private CharacterController control;
        [SerializeField] private GameObject LookTarget;

        [Range(0.1f, 20f)]
        [SerializeField] private float Speed = 1f;


        void Update()
        {
            // Apply movement based on direction obtained
            control.SimpleMove(steer.MoveDirection() * Speed);

            // Look towards target
            transform.rotation = Quaternion.LookRotation(MapOperations.VectorToTarget(gameObject, LookTarget).normalized);
        }


#if UNITY_EDITOR
        private void OnDrawGizmos()
        {

            Handles.color = Color.magenta;
            if (steer.MoveDirection() != Vector3.zero)
                Handles.ArrowHandleCap(0, transform.position, Quaternion.LookRotation(steer.MoveDirection(), Vector3.up), 2f, EventType.Repaint);
        }
#endif
    }

}

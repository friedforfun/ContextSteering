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
        [SerializeField] public GameObject LookTarget;

        [Tooltip("Movement speed of the agent.")]
        [Range(0.1f, 20f)]
        [SerializeField] private float Speed = 1f;

        [Tooltip("Minimum sqrMagnitute of direction vector to allow movement, higher values can reduce jittery movement.")]
        [Range(0.001f, 0.5f)]
        [SerializeField] private float ConfidenceThreshold = 0.1f;

        void Update()
        {

            Vector3 moveVec = steer.MoveVector();
            if (moveVec.sqrMagnitude > ConfidenceThreshold)
                control.SimpleMove(steer.MoveDirection() * Speed);
            else
                control.SimpleMove(Vector3.zero);

            // Look towards target
            Vector3 newRotation;
            if (LookTarget != null)
            {
                 newRotation = Quaternion.LookRotation(MapOperations.VectorToTarget(gameObject, LookTarget).normalized).eulerAngles;
            }
            else if (!moveVec.Equals(Vector3.zero))
            {
                newRotation = Quaternion.LookRotation(steer.MoveDirection()).eulerAngles;
            } else
            {
                newRotation = Quaternion.LookRotation(transform.forward).eulerAngles;
            }
                
            transform.rotation = Quaternion.Euler(0, newRotation.y, 0);
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

using UnityEngine;
using Friedforfun.ContextSteering.PlanarMovement;
using Friedforfun.ContextSteering.Utilities;
using UnityEditor;

namespace Friedforfun.ContextSteering.Demo
{
    /// <summary>
    /// This demo movement script shows obtaining data from the PlanarSteeringController and using it to move, you will want to do something similar in your own project.
    /// </summary>
    public class PlanarMovement : MonoBehaviour
    {
        [SerializeField] private PlanarSteeringController steer;
        [SerializeField] private CharacterController control;
        [SerializeField] public GameObject LookTarget;


        [Tooltip("Movement speed of the agent.")]
        [Range(0.1f, 20f)]
        [SerializeField] private float Speed = 1f;

        [Tooltip("Minimum sqrMagnitute of direction vector to allow movement, higher values can reduce jittery movement but may also stop the agent moving when you might want it to.")]
        [Range(0.001f, 0.5f)]
        [SerializeField] private float ConfidenceThreshold = 0.1f;

        void Update()
        {
            // --------------------- Example for using this package ----------------------------

            Vector3 moveVec = steer.MoveVector(); // In this case we look at the Movement Vector so we can evaluate how close to 0 it is and ocasionally remove some jitter, this evaluation is not always needed.

            if (moveVec.sqrMagnitude > ConfidenceThreshold)
                control.SimpleMove(steer.MoveDirection() * Speed); // This line gets the movement vector from the Context steering controller.
            else
                control.SimpleMove(Vector3.zero);
            // -------------------------------------------------------------------------------------


            // This just handles rotating the gameObject
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

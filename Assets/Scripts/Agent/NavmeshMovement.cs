using UnityEngine;
using UnityEngine.AI;
using Friedforfun.SteeringBehaviours.Core2D;

namespace Friedforfun.SteeringBehaviours.Demo
{
    public class NavmeshMovement : MonoBehaviour
    {
        [SerializeField] private BaseContextSteering2D steer;
        [SerializeField] private CharacterController control;

        [Range(0.1f, 20f)]
        [SerializeField] private float Speed = 1f;

        private NavMeshPath path;

        private void Start()
        {
            path = new NavMeshPath();
        }

        private bool updatePath(Vector3 targetPosition)
        {
            return NavMesh.CalculatePath(transform.position, targetPosition, NavMesh.AllAreas, path);
        }
    }

}

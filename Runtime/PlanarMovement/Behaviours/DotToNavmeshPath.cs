using UnityEngine;
using UnityEngine.AI;
using Friedforfun.ContextSteering.Core;
using Friedforfun.ContextSteering.Utilities;

namespace Friedforfun.ContextSteering.PlanarMovement
{
    public class DotToNavmeshPath : PlanarSteeringBehaviour
    {
        [Tooltip("Min distance Squared from each target location to move to the next corner in the path.")]
        [SerializeField] public float SqrWaypointRadius = 0.01f;

        [Tooltip("Enable defining a custom target, useful for testing.")]
        [SerializeField] bool serializedTarget = false;
        [Tooltip("Custom target destination, will generate a navmesh path for this target.")]
        [SerializeField] private Vector3 TargetDestination;
        [Tooltip("Navmesh area mask, used to determine which navmesh areas we have avaliable.")]
        [SerializeField] public int AreaMask = NavMesh.AllAreas;

        private bool hasPath = false;
        private NavMeshPath path;
        private int pathIndex;

        private void Start()
        {
            if (serializedTarget)
            {
                CalculatePath(TargetDestination);
            }
        }

        /// <summary>
        /// Attempt to compute a path to the target destination on the navmesh
        /// </summary>
        /// <param name="targetDestination"></param>
        /// <returns>True if either a complete or partial path is found</returns>
        public bool CalculatePath(Vector3 targetDestination)
        {
            path = new NavMeshPath();
            hasPath = NavMesh.CalculatePath(transform.position, targetDestination, AreaMask, path);
            if (hasPath)
            {
                pathIndex = 0;
            }
            return hasPath;
        }

        /// <summary>
        /// Clear the current path from this behaviour
        /// </summary>
        public void ClearPath()
        {
            hasPath = false;
        }

        protected override Vector3[] getPositionVectors()
        {
            return new Vector3[] { currentCorner()};
        }

        private Vector3 currentCorner()
        {
            if (!hasPath) // Either no path is stored, or no path can be computed
            {
                return transform.position;
            }
            float sqrMag = MapOperations.VectorToTarget(transform.position, path.corners[pathIndex]).sqrMagnitude;
            if (sqrMag < SqrWaypointRadius && pathIndex < path.corners.Length - 1)
            {
                pathIndex++;
                return path.corners[pathIndex];  
            }
            // otherwise there are no more corners or we have not yet reached the next corner
            return path.corners[pathIndex];

        }

        /// <summary>
        /// Custom Debug view of path.
        /// </summary>
#if UNITY_EDITOR
        protected override void OnDrawGizmos()
        {
            base.OnDrawGizmos();
            if (hasPath && MapDebugger != null)
                if (MapDebugger.ShowDebug)
                    for (int i = 0; i < path.corners.Length - 1; i++)
                        Debug.DrawLine(path.corners[i], path.corners[i + 1], MapDebugger.DebugColor);

        }
#endif
    }

}

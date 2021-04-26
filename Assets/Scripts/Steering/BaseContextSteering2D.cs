using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using System.Collections.Concurrent;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class BaseContextSteering2D : MonoBehaviour
{
    [Header("Behaviours")]
    [Range(4, 32)]
    public int ContextMapResolution = 12; // Number of directions the context map represents
    public SteeringBehaviour[] SteeringBehaviours; // Attractor and Repulsor strategies
    public SteeringMask[] SteeringMasks; // Mask strategies
    protected ICombineContext ContextCombinator; // strategy for combining steering and mask maps
    protected IDecideDirection DirectionDecider; // strategy for selecting the direction to move in based on the context map

    [Header("Debug")]
    [SerializeField] private float DebugRadius;

    private float resolutionAngle;
    private float[] contextMap;
    private Vector3 lastVector;

    /// <summary>
    /// Gets the most desired direction to move in. Normalised.
    /// </summary>
    /// <returns></returns>
    public Vector3 MoveDirection()
    {
        contextMap = ContextCombinator.CombineContext(buildSteeringBehaviours(), buildSteeringMasks());
        lastVector = DirectionDecider.GetDirection(contextMap, lastVector);
        return lastVector.normalized;
    }

    /// <summary>
    /// Should not be used directly as a move input.
    /// Get the direction and magnitude (not normalised) of the most desired direction, use depends on behaviour implementations.
    /// </summary>
    /// <returns></returns>
    public Vector3 MoveVector()
    {
        contextMap = ContextCombinator.CombineContext(buildSteeringBehaviours(), buildSteeringMasks());
        lastVector = DirectionDecider.GetDirection(contextMap, lastVector);
        return lastVector;
    }


    private void Start()
    {
        resolutionAngle = 360 / (float)ContextMapResolution;

        foreach (SteeringBehaviour behaviour in SteeringBehaviours)
        {
            behaviour.InstantiateContextMap(ContextMapResolution);
        }

        foreach(SteeringMask mask in SteeringMasks)
        {
            mask.InstantiateMaskMap(ContextMapResolution);
        }
    }

    
    /// <summary>
    /// Builds all steering behaviours and sums each weight elementwise, to determine the most desirable direction to move in.
    /// </summary>
    /// <returns></returns>
    private float[] buildSteeringBehaviours()
    {
        ConcurrentBag<float[]> contextMaps = new ConcurrentBag<float[]>();

        Parallel.ForEach(SteeringBehaviours, behaviour =>
        {
            contextMaps.Add(behaviour.BuildContextMap());
        });
        
        return mergeMaps(contextMaps);
    }

    /// <summary>
    /// Build a context mask of directions to block movement in this direction
    /// </summary>
    /// <returns></returns>
    private float[] buildSteeringMasks()
    {
        ConcurrentBag<float[]> contextMaps = new ConcurrentBag<float[]>();

        Parallel.ForEach(SteeringMasks, behaviour =>
        {
            contextMaps.Add(behaviour.BuildMaskMap());
        });

        return mergeMaps(contextMaps);
    }

    /// <summary>
    /// Merge bag of context maps by summing each element
    /// </summary>
    /// <param name="maps"></param>
    /// <returns></returns>
    private float[] mergeMaps(ConcurrentBag<float[]> maps)
    {
        float[] contextMap = new float[ContextMapResolution];

        foreach (float[] map in maps)
        {
            contextMap.Zip(map, (x, y) => x + y);
        }

        return contextMap;
    }


#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Vector3 position = transform.position;
        Vector3 direction = Vector3.forward;

        for (int i = 0; i < ContextMapResolution; i++)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawRay(transform.position, direction);
            direction = Quaternion.Euler(0f, resolutionAngle, 0) * direction;
        }

        Handles.DrawWireDisc(position, Vector3.up, DebugRadius);
    }
#endif
}

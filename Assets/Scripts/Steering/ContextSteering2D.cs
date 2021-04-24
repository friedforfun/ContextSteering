using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using System.Collections.Concurrent;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif

public enum BuildContextMapOn
{
    UPDATE,
    FIXED_UPDATE,
    TIME
}

public class Steering2D : MonoBehaviour
{
    [Header("Behaviours")]
    public int ContextMapResolution; // Number of directions the context map represents
    public ISteeringBehaviour[] SteeringBehaviours; // Attractors and Repulsors
    public ISteeringMask[] SteeringMasks;
    public BuildContextMapOn BuildMapOn;

    [Header("Gizmo")]
    [SerializeField] private float Radius;

    private float resolutionAngle; // Each point is separeted by a some degrees rotation (360/len(chaseMap))

    private void Start()
    {
        resolutionAngle = 360 / (float)ContextMapResolution;
    }

    private void Update()
    {
        if (BuildMapOn != BuildContextMapOn.UPDATE)
        {
            return;
        }
    }

    private void FixedUpdate()
    {
        if (BuildMapOn != BuildContextMapOn.FIXED_UPDATE)
        {
            return;
        }
    }

    
    /// <summary>
    /// Builds a context map of directions towards the target from all steering behaviours
    /// </summary>
    /// <returns></returns>
    private float[] buildSteeringBehaviours()
    {
        ConcurrentBag<float[]> contextMaps = new ConcurrentBag<float[]>();

        Parallel.ForEach(SteeringBehaviours, behaviour =>
        {
            contextMaps.Add(behaviour.BuildContextMap());
        });
        
        return mergeContextMaps(contextMaps);
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
            contextMaps.Add(behaviour.BuildContextMask());
        });

        return mergeContextMaps(contextMaps);
    }

    /// <summary>
    /// Merge bag of context maps
    /// </summary>
    /// <param name="maps"></param>
    /// <returns></returns>
    private float[] mergeContextMaps(ConcurrentBag<float[]> maps)
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


        Handles.DrawWireDisc(position, Vector3.up, Radius);
    }
#endif
}

using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

/// <summary>
/// Singleton registry of gameobjects, use instead of FindObjectByTag
/// GameObjects need to register themselves
/// </summary>
public class ReferencePool : MonoBehaviour
{

    private static readonly Lazy<ReferencePool> singleton = new Lazy<ReferencePool>(() => Init(), LazyThreadSafetyMode.ExecutionAndPublication);
    private static ReferencePool instance { get { return singleton.Value;  } }
    private static ReferencePool Init()
    {
        ReferencePool tagRegistry = FindObjectOfType(typeof(ReferencePool)) as ReferencePool;
        if (!tagRegistry)
        {
            Debug.LogError("Attempted to access instance of ReferencePool but it cannot be found in the scene");
        }
        else
        {
            if (tagRegistry.registeredTags == null)
            {
                tagRegistry.registeredTags = new Dictionary<string, List<GameObject>>();
            }

            if (tagRegistry.positionCache == null)
            {
                tagRegistry.positionCache = new Dictionary<string, Vector3[]>();
            }
        }
        
        return tagRegistry;

    }


    private Dictionary<string, List<GameObject>> registeredTags = new Dictionary<string, List<GameObject>>();
    private Dictionary<string, Vector3[]> positionCache = new Dictionary<string, Vector3[]>();

    public static void Register(GameObject go)
    {
        List<GameObject> go_list = null;
        if (!instance.registeredTags.TryGetValue(go.tag, out go_list))
        {
            go_list = new List<GameObject>();
            instance.registeredTags[go.tag] = go_list;
        }

        if (!go_list.Contains(go))
            go_list.Add(go);
    }

    public static void DeRegister(GameObject go)
    {
        List<GameObject> go_list = null;
        if (instance.registeredTags.TryGetValue(go.tag, out go_list))
        {
            go_list.Remove(go);
        }
        else
        {
            Debug.LogWarning($"{go.name} attempted to deregister itself, but it is not in the ReferencePool");
        }
    }

    public static GameObject[] GetGameObjectsByTag(string tag)
    {
        List<GameObject> go_list = null;
        if (!instance.registeredTags.TryGetValue(tag, out go_list))
        {
            Debug.Log("(Remove this from ReferencePool.cs) No gameobjects found for this tag");
            go_list = new List<GameObject>();
        }

        return go_list.ToArray();
    }

    private void LateUpdate()
    {
        instance.positionCache.Clear();
    }

    private void Start()
    {
        List<GameObject> targets = null;
        if (instance.registeredTags.TryGetValue("Target", out targets))
        {
            Debug.Log($"Got {targets.Count} targets in list with following:");
            foreach (GameObject t in targets)
            {
                Debug.Log($"Target: {t.name}, Position: {t.transform.position}");
            }
        }
    }

    /// <summary>
    /// Get position vectors for all GameObjects of this tag, caches result for this frame.
    /// </summary>
    /// <param name="tag"></param>
    /// <returns></returns>
    public static Vector3[] GetVector3sByTag(string tag)
    {
        Vector3[] pos_arr = null;
        if (!instance.positionCache.TryGetValue(tag, out pos_arr))
        {

            GameObject[] tempTargets = GetGameObjectsByTag(tag);
            
            pos_arr = new Vector3[tempTargets.Length];

            for (int i = 0; i < pos_arr.Length; i++)
            {
                pos_arr[i] = tempTargets[i].transform.position;
            }

            instance.positionCache[tag] = pos_arr;
        }

        return pos_arr;
    }

    /// <summary>
    /// Get position vectors for all GameObjects of this tag, caches result for this frame. Self is excluded in array of Vector3s
    /// </summary>
    /// <param name="tag"></param>
    /// <returns></returns>
    public static Vector3[] GetVector3sByTag(string tag, GameObject self)
    {
        Vector3[] pos_arr = null;
        if (!instance.positionCache.TryGetValue(tag, out pos_arr))
        {


            GameObject[] tempTargets = GetGameObjectsByTag(tag);
            if (self.CompareTag(tag))
            {
                pos_arr = new Vector3[tempTargets.Length-1];
            }
            else
            {
                pos_arr = new Vector3[tempTargets.Length];
            }

            for (int i = 0; i < pos_arr.Length; i++)
            {
                if (!self.Equals(tempTargets[i]))
                    pos_arr[i] = tempTargets[i].transform.position;
            }

            instance.positionCache[tag] = pos_arr;
        }

        return pos_arr;
    }

}

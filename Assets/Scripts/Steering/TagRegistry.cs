using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

/// <summary>
/// Singleton registry of gameobjects, use instead of FindObjectByTag
/// GameObjects need to register themselves
/// </summary>
public class TagRegistry : MonoBehaviour
{

    private static readonly Lazy<TagRegistry> singleton = new Lazy<TagRegistry>(() => Init(), LazyThreadSafetyMode.ExecutionAndPublication);
    private static TagRegistry instance { get { return singleton.Value;  } }
    private static TagRegistry Init()
    {
        TagRegistry tagRegistry = FindObjectOfType(typeof(TagRegistry)) as TagRegistry;
        if (!tagRegistry)
        {
            Debug.LogError("Attempted to access instance of TagRegistry but it cannot be found in the scene");
        }
        else if (tagRegistry.registeredTags == null)
        {
            tagRegistry.registeredTags = new Dictionary<string, List<GameObject>>();
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
            Debug.LogWarning($"{go.name} attempted to deregister itself, but it is not in the TagRegistry");
        }
    }

    public static GameObject[] GetGameObjectsByTag(string tag)
    {
        List<GameObject> go_list = null;
        if (!instance.registeredTags.TryGetValue(tag, out go_list))
        {
            Debug.Log("(Remove this from TagRegistry.cs) No gameobjects found for this tag");
            go_list = new List<GameObject>();
        }

        return go_list.ToArray();
    }

    private void LateUpdate()
    {
        instance.positionCache.Clear();
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
            int oldLen = 0;

            GameObject[] tempTargets = GetGameObjectsByTag(tag);
            
            pos_arr = new Vector3[tempTargets.Length];

            for (int i = oldLen; i < pos_arr.Length; i++)
            {
                pos_arr[i] = tempTargets[i - oldLen].transform.position;
            }

            instance.positionCache[tag] = pos_arr;
        }

        return pos_arr;
    }

}

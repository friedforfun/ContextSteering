using System.Collections.Generic;
using UnityEngine;
using System.Linq;


namespace Friedforfun.ContextSteering.Utilities
{
    /// <summary>
    /// Singleton registry of gameobjects, use instead of FindObjectByTag, this also caches requested position Vectors each frame
    /// GameObjects need to register themselves
    /// </summary>
    public class TagCache : Singleton<TagCache>
    {

        private static Dictionary<string, List<GameObject>> registeredTags = new Dictionary<string, List<GameObject>>();
        private static Dictionary<string, Vector3[]> positionCache = new Dictionary<string, Vector3[]>();

        private static bool isGameObject = false;

        /// <summary>
        /// Register GameObject in the pool of registered tag->GameObjects. Usually register on Awake()/Enable()/Start()
        /// </summary>
        /// <param name="go">The GameObject being registered</param>
        public static void Register(GameObject go)
        {
            List<GameObject> go_list = null;
            if (!registeredTags.TryGetValue(go.tag, out go_list))
            {
                go_list = new List<GameObject>();
                registeredTags[go.tag] = go_list;
            }

            if (!go_list.Contains(go))
                go_list.Add(go);
        }

        /// <summary>
        /// Remove this GameObject from the pool. Usually Use this in OnDisable()/OnDestroy()
        /// </summary>
        /// <param name="go">The GameObject being removed</param>
        public static void DeRegister(GameObject go)
        {
            List<GameObject> go_list = null;
            if (registeredTags.TryGetValue(go.tag, out go_list))
            {
                go_list.Remove(go);
            }
            else
            {
                Debug.LogWarning($"{go.name} attempted to deregister itself, but it is not in the TagCache");
            }
        }

        /// <summary>
        /// Gets an array of GameObjects by providing a tag, this will only check for GameObjects that have been Registered to the TagCache class.
        /// </summary>
        /// <param name="tag">The tag we are checking</param>
        /// <returns>All GameObjects with the corresponding tag within the TagCache</returns>
        public static GameObject[] GetGameObjectsByTag(string tag)
        {
            List<GameObject> go_list = null;
            if (!registeredTags.TryGetValue(tag, out go_list))
            {
                go_list = new List<GameObject>();
            }

            return go_list.ToArray();
        }

        private void LateUpdate()
        {
            ClearCache();
        }

        /// <summary>
        /// Clears all positions stored in the position cache
        /// </summary>
        public static void ClearCache()
        {
            positionCache.Clear();
        }

        /// <summary>
        /// Clears all registered tags and position cache
        /// </summary>
        public static void ClearAll()
        {
            positionCache.Clear();
            registeredTags.Clear();
        }

        /// <summary>
        /// Get position vectors for all GameObjects of this tag that have been registered in the TagCache, caches result for this frame.
        /// </summary>
        /// <param name="tag"></param>
        /// <returns>The positions of all registered GameObjects with the matching tags</returns>
        public static Vector3[] GetVector3sByTag(string tag)
        {
            CheckIfInScene();
            Vector3[] pos_arr = null;
            if (!positionCache.TryGetValue(tag, out pos_arr))
            {

                GameObject[] tempTargets = GetGameObjectsByTag(tag);

                pos_arr = new Vector3[tempTargets.Length];

                for (int i = 0; i < pos_arr.Length; i++)
                {
                    pos_arr[i] = tempTargets[i].transform.position;
                }

                positionCache[tag] = pos_arr;
            }

            return pos_arr;
        }

        /// <summary>
        /// Get position vectors for all GameObjects of this tag, caches result for this frame. Self is excluded in result array of Vector3s
        /// </summary>
        /// <param name="tag">The tag we are checking</param>
        /// <param name="self">The GameObject calling this method, the position of self will be excluded from the result.</param>
        /// <returns>The positions of all registered GameObjects with the matching tags excluding self.transform.position</returns>
        public static Vector3[] GetVector3sByTag(string tag, GameObject self)
        {
            CheckIfInScene();
            Vector3[] pos_arr = GetVector3sByTag(tag);
            Vector3 selfLoc = self.transform.position;
            Vector3[] cleanArr = pos_arr.Where(value => value != selfLoc).ToArray();

            return cleanArr;
        }

        /// <summary>
        /// Same as GetVector3sByTag but without caching the result.
        /// </summary>
        /// <param name="tag">The tag we are checking</param>
        /// <returns>The positions of all registered GameObjects with the matching tags.</returns>
        public static Vector3[] GetVector3sByTagNoCache(string tag)
        {
            GameObject[] tempTargets = GetGameObjectsByTag(tag);

            Vector3[] pos_arr = new Vector3[tempTargets.Length];

            for (int i = 0; i < pos_arr.Length; i++)
            {
                pos_arr[i] = tempTargets[i].transform.position;
            }

            return pos_arr;
        }

        /// <summary>
        /// Same as GetVector3sByTag but without caching the result this frame.
        /// </summary>
        /// <param name="tag">The tag we are checking</param>
        /// <param name="self">The GameObject calling this method, the position of self will be excluded from the result.</param>
        /// <returns>The positions of all registered GameObjects with the matching tags excluding self.transform.position</returns>
        public static Vector3[] GetVector3sByTagNoCache(string tag, GameObject self)
        {
            Vector3[] pos_arr = GetVector3sByTagNoCache(tag);
            Vector3 selfLoc = self.transform.position;
            Vector3[] cleanArr = pos_arr.Where(value => value != selfLoc).ToArray();

            return cleanArr;
        }

        private static void CheckIfInScene()
        {
            if (isGameObject)
                return;

            TagCache me = FindObjectOfType<TagCache>();
            if (me != null)
            {
                isGameObject = true;
                return;
            }

            Debug.LogWarning("No TagCache found in scene, Cached position Vector3s will not be cleared each frame. If you dont wish to use the TagCache consider creating a custom DotToTag script.");

        }

    }
}


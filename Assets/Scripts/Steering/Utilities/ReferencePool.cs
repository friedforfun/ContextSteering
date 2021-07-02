using System.Collections.Generic;
using UnityEngine;


namespace Friedforfun.SteeringBehaviours.Utilities
{
    /// <summary>
    /// Singleton registry of gameobjects, use instead of FindObjectByTag
    /// GameObjects need to register themselves
    /// </summary>
    public class ReferencePool : Singleton<ReferencePool>
    {

        private Dictionary<string, List<GameObject>> registeredTags = new Dictionary<string, List<GameObject>>();
        private Dictionary<string, Vector3[]> positionCache = new Dictionary<string, Vector3[]>();

        public static void Register(GameObject go)
        {
            List<GameObject> go_list = null;
            if (!Instance.registeredTags.TryGetValue(go.tag, out go_list))
            {
                go_list = new List<GameObject>();
                Instance.registeredTags[go.tag] = go_list;
            }

            if (!go_list.Contains(go))
                go_list.Add(go);
        }

        public static void DeRegister(GameObject go)
        {
            List<GameObject> go_list = null;
            if (Instance.registeredTags.TryGetValue(go.tag, out go_list))
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
            if (!Instance.registeredTags.TryGetValue(tag, out go_list))
            {
                Debug.Log("(Remove this from ReferencePool.cs) No gameobjects found for this tag");
                go_list = new List<GameObject>();
            }

            return go_list.ToArray();
        }

        private void LateUpdate()
        {
            Instance.positionCache.Clear();
        }

        /// <summary>
        /// Get position vectors for all GameObjects of this tag, caches result for this frame.
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public static Vector3[] GetVector3sByTag(string tag)
        {
            Vector3[] pos_arr = null;
            if (!Instance.positionCache.TryGetValue(tag, out pos_arr))
            {

                GameObject[] tempTargets = GetGameObjectsByTag(tag);

                pos_arr = new Vector3[tempTargets.Length];

                for (int i = 0; i < pos_arr.Length; i++)
                {
                    pos_arr[i] = tempTargets[i].transform.position;
                }

                Instance.positionCache[tag] = pos_arr;
            }

            return pos_arr;
        }

        /// <summary>
        /// 
        /// Get position vectors for all GameObjects of this tag, caches result for this frame. Self is excluded in result array of Vector3s
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public static Vector3[] GetVector3sByTag(string tag, GameObject self)
        {
            Vector3[] pos_arr = GetVector3sByTag(tag);
            Vector3[] cleanArr = new Vector3[pos_arr.Length - 1];
            Vector3 selfLoc = self.transform.position;

            for (int i = 0; i < pos_arr.Length; i++)
            {
                if (MapOperations.VectorToTarget(selfLoc, pos_arr[i]).magnitude > 0.05f)
                    cleanArr[i] = pos_arr[i];// array index mismatch when skip one element
            }

            return cleanArr;
        }

    }
}

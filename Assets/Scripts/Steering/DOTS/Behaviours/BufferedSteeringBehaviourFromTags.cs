using System;
using UnityEngine;

public abstract class BufferedSteeringBehaviourFromTags: BufferedSteeringBehaviour
{
    [SerializeField] string[] Tags;

    protected Vector3[] getTargetVectors()
    {
        int oldLen = 0;
        Vector3[] targets = null;
        foreach (string tag in Tags)
        {
            Vector3[] tempTargets = ReferencePool.GetVector3sByTag(tag);

            if (targets == null)
            {
                targets = new Vector3[tempTargets.Length];

            }
            else
            {
                oldLen = targets.Length;
                Array.Resize(ref targets, targets.Length + tempTargets.Length);
            }

            // Copy new elements into the start of the space added by Array.Resize, or the start of the array if its empty
            Array.Copy(tempTargets, 0, targets, oldLen, tempTargets.Length);

        }
        return targets;
    }

}

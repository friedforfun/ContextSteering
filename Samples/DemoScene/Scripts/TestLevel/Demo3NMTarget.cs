using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Friedforfun.ContextSteering.Demo
{
    public class Demo3NMTarget : MonoBehaviour
    {
        [SerializeField]
        Vector3[] Positions;

        int index = 0;
        private void OnTriggerEnter(Collider other)
        {
            Demo3NavmeshBuilder dnmb = other.GetComponent<Demo3NavmeshBuilder>();
            if (dnmb != null)
            {
                index++;

                if (index == Positions.Length)
                    index = 0;

                transform.localPosition = Positions[index];
                StartCoroutine(buildPath(dnmb));
            }

            IEnumerator buildPath(Demo3NavmeshBuilder dnmb)
            {
                yield return new WaitForSeconds(0.1f);
                dnmb.HitGoal();
            }

        }
    }
}
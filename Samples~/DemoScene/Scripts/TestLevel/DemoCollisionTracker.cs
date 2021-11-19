using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Friedforfun.ContextSteering.Demo
{
    public class DemoCollisionTracker : MonoBehaviour
    {
        [SerializeField]
        public string DemoID;

        [SerializeField]
        int timeIntervalSec = 15;

        [SerializeField] 
        TextMeshPro TotalCollisions;

        [SerializeField]
        TextMeshPro CollisionsPerMin;

        [SerializeField]
        TextMeshPro aveCollisionText;

        [SerializeField]
        TextMeshPro goalsText;

        int totalCollisions = 0;
        int collisionsInLastMin = 0;
        float aveCollisions = 0f;
        int goals = 0;

        private void Start()
        {
            StartCoroutine(computeAve());
        }

        public void CollisionOccured()
        {
            totalCollisions++;
            collisionsInLastMin++;
            StartCoroutine(deductCollisionAfterInterval());
            if (TotalCollisions != null && CollisionsPerMin != null)
            {
                TotalCollisions.text = $"Total Collisions: {totalCollisions}";
                CollisionsPerMin.text = $"In last {timeIntervalSec}s: {collisionsInLastMin}";
            }
        }

        public void GoalAchieved()
        {
            goals++;
            if (goalsText != null)
                goalsText.text = $"Goals: {goals}";
        }

        IEnumerator computeAve()
        {
            yield return new WaitForSecondsRealtime(timeIntervalSec);
            int aveTicks = 0;
            for (; ; )
            {
                aveTicks++;
                aveCollisions = (float) totalCollisions / (float) aveTicks;
                if (aveCollisionText != null)
                    aveCollisionText.text = $"Average: {aveCollisions}";
                yield return new WaitForSecondsRealtime(timeIntervalSec);
            }
        }

        IEnumerator deductCollisionAfterInterval()
        {
            yield return new WaitForSecondsRealtime(timeIntervalSec);
            collisionsInLastMin--;
            if (CollisionsPerMin != null)
            {
                CollisionsPerMin.text = $"In last {timeIntervalSec}s: {collisionsInLastMin}";
            }
        }
    }

}

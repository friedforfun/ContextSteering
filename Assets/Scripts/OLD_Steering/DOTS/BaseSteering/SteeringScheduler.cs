using System;
using System.Linq;
using System.Threading;
using System.Collections.Generic;
using UnityEngine;

namespace Friedforfun.SteeringBehaviours.Core2D.Buffered
{
    public class SteeringScheduler : MonoBehaviour
    {
        private static readonly Lazy<SteeringScheduler> singleton = new Lazy<SteeringScheduler>(() => Init(), LazyThreadSafetyMode.ExecutionAndPublication);

        private static SteeringScheduler instance { get { return singleton.Value; } }

        private static SteeringScheduler Init()
        {
            SteeringScheduler steeringScheduler = FindObjectOfType(typeof(SteeringScheduler)) as SteeringScheduler;
            if (!steeringScheduler)
            {
                // could refactor to non-singleton pattern, depends on usecase
                Debug.LogError("No instances of SteeringScheduler found in scene.");
            }

            DontDestroyOnLoad(steeringScheduler);
            return steeringScheduler;

        }

        BaseContextSteering2DBuffered[] Steerers;

        [Tooltip("How many ways to split up the behaviour updates, higher values will make the agents change direction less frequently but reduce the burden on the system.")]
        [SerializeField] int SchedulingGroups = 1;
        int currentGroupIndex = 0;

        int groupSize;
        // Needs to tell all the steerers to schedules their jobs

        private void Start()
        {
            RepopulateSteerers();
        
        }

        public static void RepopulateSteerers()
        {
            instance.Steerers = FindObjectsOfType<BaseContextSteering2DBuffered>();
        }


        private static void ScheduleBehavioursGrouped()
        {

            int gSize = instance.groupSize;

            for (int i = instance.currentGroupIndex * gSize; i < (gSize * (instance.currentGroupIndex + 1)); i++)
            {
                if (i < instance.Steerers.Length)
                    instance.Steerers[i].ScheduleJobs();
            }
        }


        private static void CompleteBehavioursGrouped()
        {
            int gSize = instance.groupSize;


            for (int i = instance.currentGroupIndex * gSize; i < gSize * (instance.currentGroupIndex + 1); i++)
            {
                if (i < instance.Steerers.Length)
                    instance.Steerers[i].CompleteJobs();
            }
            instance.currentGroupIndex++;
            if (instance.currentGroupIndex == instance.SchedulingGroups)
            {
                instance.currentGroupIndex = 0;
            }
                
        }

        private static void computeGroupSize()
        {
            instance.groupSize = (int) Mathf.Ceil((instance.Steerers.Length + 1.0f) / instance.SchedulingGroups);
        }

        private static void ScheduleBehaviours()
        {
            foreach (BaseContextSteering2DBuffered s in instance.Steerers)
            {
                //Debug.Log($"Scheduling steering on: {s.gameObject.name}");
                s.ScheduleJobs();
            }
        }

        // needs to tell all the steerers to complete their jobs
        private void CompleteBehaviours()
        {
            foreach (BaseContextSteering2DBuffered s in Steerers)
            {
                s.CompleteJobs();
            }
        }


        private void FixedUpdate()
        {
            computeGroupSize();

            ScheduleBehavioursGrouped();

            CompleteBehavioursGrouped();
        }

    }

}

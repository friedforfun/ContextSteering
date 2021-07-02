using System;
using System.Linq;
using System.Threading;
using System.Collections.Generic;
using UnityEngine;

namespace Friedforfun.SteeringBehaviours.Core2D.Buffered
{

    public delegate void DuringContextUpdate();

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


        public DuringContextUpdate duringContextUpdate;

        BaseContextSteering2DBuffered[] Steerers;

        [SerializeField] int SchedulingGroups = 1;
        int currentGroupIndex = 0;

        int groupSize;
        // Needs to tell all the steerers to schedules their jobs

        private void Start()
        {
            RepopulateSteerers();
        
        }

        public static int AmIInSteerers(BaseContextSteering2DBuffered self)
        {
            List<BaseContextSteering2DBuffered> steeringList = instance.Steerers.ToList();
            return steeringList.IndexOf(self);
        }

        public static void RepopulateSteerers()
        {
            instance.Steerers = FindObjectsOfType<BaseContextSteering2DBuffered>();
        }


        private static void ScheduleBehavioursTest()
        {

            int gSize = instance.groupSize;

            for (int i = instance.currentGroupIndex * gSize; i < (gSize * (instance.currentGroupIndex + 1)); i++)
            {
                if (i < instance.Steerers.Length)
                    instance.Steerers[i].ScheduleJobs();
            }
        }


        private static void CompleteBehavioursTest()
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

            ScheduleBehavioursTest();

            CompleteBehavioursTest();
        }

        const float fpsMeasurePeriod = 0.5f;
        private int m_FpsAccumulator = 0;
        private float m_FpsNextPeriod = 0;
        private int m_CurrentFps = 0;

        public int Measure()
        {
            // measure average frames per second
            m_FpsAccumulator++;
            if (Time.realtimeSinceStartup > m_FpsNextPeriod)
            {
                m_CurrentFps = (int)(m_FpsAccumulator / fpsMeasurePeriod);
                m_FpsAccumulator = 0;
                m_FpsNextPeriod += fpsMeasurePeriod;
            }
            return m_CurrentFps;
        }

    }

}

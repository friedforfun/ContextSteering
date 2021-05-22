using System;
using System.Threading;
using System.Collections.Generic;
using UnityEngine;

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
        else
        {
            //steeringScheduler.fps =  new FPSCounter();
        }

        return steeringScheduler;

    }


    public DuringContextUpdate duringContextUpdate;

    BaseContextSteering2DBuffered[] Steerers;

    [SerializeField] int ScheduleRatePerSecond = 10;
    bool evaluateGroups = true;
    int currentGroupIndex = 0;
    int numberPerGroup = instance.Steerers.Length;

   // FPSCounter fps;
    int CurrentFPS = 0;
    // Needs to tell all the steerers to schedules their jobs

    private void Start()
    {
        RepopulateSteerers();
        numberPerGroup = instance.Steerers.Length;
    }

    public static void RepopulateSteerers()
    {
        instance.Steerers = FindObjectsOfType<BaseContextSteering2DBuffered>();
    }


    private static void ScheduleBehaviours()
    {
        if (instance.evaluateGroups)
            instance.numberPerGroup = (int)Mathf.Ceil((instance.Steerers.Length / instance.CurrentFPS) * instance.ScheduleRatePerSecond);

        instance.evaluateGroups = false;

        for (int i = instance.currentGroupIndex * instance.numberPerGroup; i < (i+1) * instance.numberPerGroup; i++)
        {
            if (i < instance.Steerers.Length)
            {
                instance.Steerers[i].ScheduleJobs();
            }
        }
    }

    // needs to tell all the steerers to complete their jobs
    private static void CompleteBehaviours()
    {
        for (int i = instance.currentGroupIndex * instance.numberPerGroup; i < (i + 1) * instance.numberPerGroup; i++)
        {
            if (i < instance.Steerers.Length)
            {
                instance.Steerers[i].CompleteJobs();
            }
        }
        instance.currentGroupIndex++;
        if (instance.currentGroupIndex*instance.numberPerGroup > instance.Steerers.Length)
        {
            instance.currentGroupIndex = 0;
            instance.evaluateGroups = true;
        }
    }

    private void Update()
    {
        CurrentFPS = Measure();
        ScheduleBehaviours();

        // onUpdate
        //duringContextUpdate();

        CompleteBehaviours();
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

    /*private class FPSCounter
    {
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
    }*/


}

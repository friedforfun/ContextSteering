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
        SteeringScheduler[] steeringSchedulers = FindObjectsOfType(typeof(SteeringScheduler)) as SteeringScheduler[];
        SteeringScheduler steeringScheduler = null;
        if (steeringSchedulers.Length != 1)
        {
            // could refactor to non-singleton pattern, depends on usecase
            Debug.LogError("Incorrect number of instances of SteeringScheduler found in scene. Should only be 1");
        }
        else
        {
            steeringScheduler = steeringSchedulers[0];
        }

        return steeringScheduler;

    }


    public DuringContextUpdate duringContextUpdate;

    BaseContextSteering2DBuffered[] Steerers;

    // Needs to tell all the steerers to schedules their jobs

    private void Start()
    {
        RepopulateSteerers();
    }

    public static void RepopulateSteerers()
    {
        instance.Steerers = FindObjectsOfType<BaseContextSteering2DBuffered>();
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

    private void Update()
    {
        ScheduleBehaviours();

        // onUpdate
        //duringContextUpdate();

        CompleteBehaviours();
    }


}

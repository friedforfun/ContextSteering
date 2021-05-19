using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void DuringContextUpdate();

public class SteeringScheduler : MonoBehaviour
{
    public DuringContextUpdate duringContextUpdate;

    BaseContextSteering2DBuffered[] Steerers;

    // Needs to tell all the steerers to schedules their jobs

    private void Awake()
    {
        RepopulateSteerers();
    }

    public void RepopulateSteerers()
    {
        Steerers = FindObjectsOfType<BaseContextSteering2DBuffered>();
    }

    private void ScheduleBehaviours()
    {
        foreach (BaseContextSteering2DBuffered s in Steerers)
        {
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

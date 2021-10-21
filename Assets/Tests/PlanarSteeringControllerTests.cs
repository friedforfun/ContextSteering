using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Friedforfun.SteeringBehaviours.Core;
using Friedforfun.SteeringBehaviours.Utilities;
using Friedforfun.SteeringBehaviours.PlanarMovement;
using Unity.Collections;
public class PlanarSteeringControllerTests
{
    GameObject agent;
    GameObject target;
    PlanarSteeringController controller;
    DotToTransform behaviour;

    [SetUp]
    public void Setup()
    {
        agent = new GameObject();
        controller = agent.AddComponent<PlanarSteeringController>();
        behaviour = agent.AddComponent<DotToTransform>();

        controller.steeringParameters = new PlanarSteeringParameters();
        controller.steeringParameters.ContextMapResolution = 4;
        controller.steeringParameters.ContextMapRotationAxis = RotationAxis.YAxis;
        controller.steeringParameters.InitialVector = Vector3.forward;

        behaviour.BehaviourName = "Test behaviour";

        target = new GameObject();
        target.transform.position = new Vector3(0, 0, 5);

        behaviour.Positions = new Transform[] { target.transform };

        controller.Awake();
        behaviour.Start();
    }

    [TearDown]
    public void TearDown()
    {
        behaviour.OnDisable();
    }

    [Test]
    public void MergeSteeringBehavioursTest()
    {

        var msb = TestUtilities.CallNonPublicMethod<float[]>(controller, "MergeSteeringBehaviours");
        Debug.Log(msb[0]);
        Assert.AreEqual(true, false);
    }

    [Test]
    public void GetJobs()
    {
        var jobs = controller.GetJobs();
        //Assert.AreEqual(TestUtilities.CallNonPublicProperty<PlanarSteeringBehaviour[]>(controller, "SteeringBehaviours").Length, jobs.Length);
        var t = TestUtilities.CallNonPublicProperty<float[]>(controller, "contextMap");
        Debug.Log(t);

        Assert.AreEqual(false, true);
    }


}


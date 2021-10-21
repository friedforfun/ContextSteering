using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Friedforfun.SteeringBehaviours.Core;
using Friedforfun.SteeringBehaviours.Utilities;
using Friedforfun.SteeringBehaviours.PlanarMovement;
using Unity.Collections;

public class DotToTransformTests
{

    GameObject agent;
    GameObject target;
    DotToTransform behaviour;

    PlanarSteeringParameters planarParameters = new PlanarSteeringParameters();

    [SetUp]
    public void Setup()
    {
        agent = new GameObject();
        behaviour = agent.AddComponent<DotToTransform>();
        agent.transform.position = new Vector3(0, 0, 0);

        planarParameters.ContextMapRotationAxis = RotationAxis.YAxis;
        planarParameters.InitialVector = new Vector3(0, 0, 1);
        planarParameters.ContextMapResolution = 4;
        behaviour.BehaviourName = "Test behaviour";

        behaviour.InstantiateContextMap(planarParameters);

        target = new GameObject();
        target.transform.position = new Vector3(0, 0, 5);

        behaviour.Positions = new Transform[] { target.transform };

        behaviour.Start();
    }

    [TearDown]
    public void TearDown()
    {
        behaviour.OnDisable();
    }

    [Test]
    public void GetJobTest()
    {    
        var job = behaviour.GetJob();

        var expectedTargets = new NativeArray<Vector3>(1, Allocator.Persistent);
        expectedTargets[0] = new Vector3(0, 0, 5);

        Assert.AreEqual(expectedTargets[0], job.targets[0]);

        Assert.AreEqual(new Vector3(0, 0, 0), job.my_position);

        Assert.AreEqual(10f, job.range);

        Assert.AreEqual(1f, job.weight);

        Assert.AreEqual(90, job.angle);

        Assert.AreEqual(SteerDirection.ATTRACT, job.direction);

        Assert.AreEqual(false, job.scaled);

        Assert.AreEqual(1f, job.invertScale);

        Assert.AreEqual(RotationAxis.YAxis, job.axis);

        // call swap to dispose of internal native array allocation
        behaviour.Swap();

        GameObject.DestroyImmediate(agent);
        expectedTargets.Dispose();
    }

    [Test]
    public void SwapTest()
    {
        var beforeMap = behaviour.GetContextMap();
        var expectedBefore = new float[] { 0f, 0f, 0f, 0f };
        Assert.AreEqual(expectedBefore, beforeMap);

        var job = behaviour.GetJob();
        job.Execute();

        behaviour.Swap();
        var result = behaviour.GetContextMap();
        var expectedAfter = new float[] { 1f, 0f, -1f, 0f };
        Assert.AreEqual(expectedAfter[0], result[0], 0.000001f);
        Assert.AreEqual(expectedAfter[1], result[1], 0.000001f);
        Assert.AreEqual(expectedAfter[2], result[2], 0.000001f);
        Assert.AreEqual(expectedAfter[3], result[3], 0.000001f);
    }

}


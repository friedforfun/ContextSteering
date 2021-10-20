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
    PlanarSteeringParameters planarParameters = new PlanarSteeringParameters();


    [Test]
    public void GetJobTest()
    {
        var agent = new GameObject();
        DotToTransform behaviour = agent.AddComponent<DotToTransform>();
        agent.transform.position = new Vector3(0, 0, 0);

        planarParameters.ContextMapRotationAxis = RotationAxis.YAxis;
        planarParameters.InitialVector = new Vector3(0, 0, 1);
        planarParameters.ContextMapResolution = 4;
        behaviour.BehaviourName = "Test behaviour";

        behaviour.InstantiateContextMap(planarParameters);

        var target = new GameObject();
        target.transform.position = new Vector3(0, 0, 1);

        behaviour.Positions = new Transform[] { target.transform };
        

        var job = behaviour.GetJob();

        var expectedTargets = new NativeArray<Vector3>(1, Allocator.Persistent);
        expectedTargets[0] = target.transform.position;

        Assert.AreEqual(expectedTargets[0], job.targets[0]);

        // call swap to dispose of internal native array allocation
        behaviour.Swap();

        GameObject.DestroyImmediate(agent);
        expectedTargets.Dispose();
    }

}


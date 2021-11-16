using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Friedforfun.ContextSteering.Core;
using Friedforfun.ContextSteering.Utilities;
using Friedforfun.ContextSteering.PlanarMovement;
using Unity.Collections;

namespace Friedforfun.ContextSteering.Tests
{

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

            TestUtilities.CallNonPublicMethod<ICombineContext>(controller, "SetContextCombinator", new BasicContextCombinator());
            TestUtilities.CallNonPublicMethod<IDecideDirection>(controller, "SetDirectionDecider", new BasicPlanarDirectionPicker(true, controller.steeringParameters));
            behaviour.BehaviourName = "Test behaviour";

            target = new GameObject();
            target.transform.position = new Vector3(0, 0, 5);

            behaviour.Positions = new Transform[] { target.transform };

            controller.Awake();
        }

        [TearDown]
        public void TearDown()
        {
            behaviour.OnDisable();
        }

        [Test]
        public void MergeMapsTest()
        {
            float[] expected = { 1.0f, 0.5f, 0f, 0.5f };

            float[] first = { 0.5f, 0.25f, 0f, 0.5f };
            float[] second = { 0.5f, 0.25f, 0f, 0f };

            List<float[]> maps = new List<float[]>();
            maps.Add(first);
            maps.Add(second);

            Assert.AreEqual(expected, TestUtilities.CallNonPublicMethod<float[]>(controller, "mergeMaps", maps));

        }

        [Test]
        public void MergeMapsEmptyTest()
        {
            float[] expected = { 0f, 0f, 0f, 0f };
            List<float[]> maps = new List<float[]>();

            Assert.AreEqual(expected, TestUtilities.CallNonPublicMethod<float[]>(controller, "mergeMaps", maps));
        }

        [Test]
        public void MergeSteeringBehavioursTest()
        {
            var jobs = controller.GetJobs();

            foreach (DotToVecJob j in jobs)
            {
                j.Execute();
            }

            foreach (PlanarSteeringBehaviour psb in TestUtilities.CallNonPublicMethod<PlanarSteeringBehaviour[]>(controller, "GetBehaviours"))
            {
                psb.Swap();
            }

            var msb = TestUtilities.CallNonPublicMethod<float[]>(controller, "MergeSteeringBehaviours");

            float[] expected = { 1f, 0f, -1f, 0f };

            Assert.AreEqual(expected[0], msb[0], TestUtilities.DOTPRODTOLERANCE);
            Assert.AreEqual(expected[1], msb[1], TestUtilities.DOTPRODTOLERANCE);
            Assert.AreEqual(expected[2], msb[2], TestUtilities.DOTPRODTOLERANCE);
            Assert.AreEqual(expected[3], msb[3], TestUtilities.DOTPRODTOLERANCE);
        }

        [Test]
        public void UpdateOutputTest()
        {
            Assert.AreEqual(Vector3.zero, controller.MoveVector());

            target.transform.position = new Vector3(5f, 0, 0);

            var jobs = controller.GetJobs();

            foreach (DotToVecJob j in jobs)
            {
                j.Execute();
            }

            foreach (PlanarSteeringBehaviour psb in TestUtilities.CallNonPublicMethod<PlanarSteeringBehaviour[]>(controller, "GetBehaviours"))
            {
                psb.Swap();
            }

            controller.UpdateOutput();
            Vector3 expected = new Vector3(1f, 0f, 0f);

            Assert.AreEqual(expected[0], controller.MoveVector()[0], TestUtilities.DOTPRODTOLERANCE);
            Assert.AreEqual(expected[1], controller.MoveVector()[1], TestUtilities.DOTPRODTOLERANCE);
            Assert.AreEqual(expected[2], controller.MoveVector()[2], TestUtilities.DOTPRODTOLERANCE);

        }

        [Test]
        public void CheckNumberOfJobs()
        {
            var jobs = controller.GetJobs();
            Assert.AreEqual(TestUtilities.CallNonPublicMethod<PlanarSteeringBehaviour[]>(controller, "GetBehaviours").Length, jobs.Length);
        }

        [Test]
        public void CheckJobPosition()
        {
            var jobs = controller.GetJobs();
            Vector3 expectedPosition = agent.transform.position;


            foreach (DotToVecJob j in jobs)
            {
                Assert.AreEqual(expectedPosition, j.my_position);
            }
        }


    }


}
using NUnit.Framework;
using UnityEngine;
using Friedforfun.ContextSteering.Utilities;
using Friedforfun.ContextSteering.PlanarMovement;
using System.Linq;
using Unity.Collections;

namespace Friedforfun.ContextSteering.Tests
{
    public class DotToLayerTests
    {

        GameObject agent;
        GameObject targetOne;
        GameObject targetTwo;
        GameObject targetThree;
        GameObject targetAbove;
        GameObject targetBelow;

        DotToLayer behaviour;

        PlanarSteeringParameters planarParameters = new PlanarSteeringParameters();

        [SetUp]
        public void Setup()
        {
            agent = new GameObject();
            behaviour = agent.AddComponent<DotToLayer>();
            agent.transform.position = new Vector3(0, 0, 0);

            planarParameters.ContextMapRotationAxis = RotationAxis.YAxis;
            planarParameters.ContextMapResolution = 4;
            behaviour.BehaviourName = "Test behaviour";

            behaviour.InstantiateContextMap(planarParameters);

            behaviour.Layers = 1 << 4 | 1 << 5 | 1 << 6;

            targetOne = new GameObject();
            targetOne.transform.position = new Vector3(0, 0, 5);
            SphereCollider colOne = targetOne.AddComponent<SphereCollider>();
            colOne.radius = 0.5f;
            targetOne.layer = 4;

            targetTwo = new GameObject();
            targetTwo.transform.position = new Vector3(5, 0, 0);
            SphereCollider colTwo = targetTwo.AddComponent<SphereCollider>();
            colTwo.radius = 0.5f;
            targetTwo.layer = 5; 

            targetThree = new GameObject();
            targetThree.transform.position = new Vector3(0, 0, 0);
            SphereCollider colThree = targetThree.AddComponent<SphereCollider>();
            colThree.radius = 0.5f;
            targetThree.layer = 6;

            targetAbove = new GameObject();
            targetAbove.transform.position = new Vector3(0, 5, 0);
            SphereCollider colAbove = targetAbove.AddComponent<SphereCollider>();
            colAbove.radius = 0.5f;
            targetAbove.layer = 6;

            targetBelow = new GameObject();
            targetBelow.transform.position = new Vector3(0, -5, 0);
            SphereCollider colBelow = targetBelow.AddComponent<SphereCollider>();
            colBelow.radius = 0.5f;
            targetBelow.layer = 6;
        }

        [TearDown]
        public void TearDown()
        {
            behaviour.OnDisable();
        }
         
        [Test]
        public void GetPositionVectorsTest()
        {
            Vector3[] positions = TestUtilities.CallNonPublicMethod<Vector3[]>(behaviour, "getPositionVectors");
            var expected = new Vector3[] { new Vector3(0f, 0f, 4.5f), new Vector3(4.5f, 0f, 0f), new Vector3(0, 0, 0), new Vector3(0, 4.5f, 0), new Vector3(0, -4.5f, 0)  };
            positions = positions.Intersect(expected).ToArray();
            Assert.AreEqual(true, positions.Contains(expected[0]));
            Assert.AreEqual(true, positions.Contains(expected[1]));
            Assert.AreEqual(true, positions.Contains(expected[2]));
            Assert.AreEqual(true, positions.Contains(expected[3]));
            Assert.AreEqual(true, positions.Contains(expected[4]));
        }

    }

}
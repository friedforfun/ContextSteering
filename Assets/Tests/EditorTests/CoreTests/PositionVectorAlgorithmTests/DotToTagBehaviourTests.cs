using NUnit.Framework;
using UnityEngine;
using Friedforfun.SteeringBehaviours.Utilities;
using Friedforfun.SteeringBehaviours.PlanarMovement;
using System.Linq;

namespace Friedforfun.SteeringBehaviours.Tests
{
    public class DotToTagTests
    {

        GameObject agent;
        GameObject targetOne;
        GameObject targetTwo;
        GameObject targetThree;

        DotToTag behaviour;

        PlanarSteeringParameters planarParameters = new PlanarSteeringParameters();

        [SetUp]
        public void Setup()
        {
            TagHelper.AddTag("Test");

            agent = new GameObject();
            behaviour = agent.AddComponent<DotToTag>();
            agent.AddComponent<ReferencePool>();
            agent.transform.position = new Vector3(0, 0, 0);

            planarParameters.ContextMapRotationAxis = RotationAxis.YAxis;
            planarParameters.InitialVector = new Vector3(0, 0, 1);
            planarParameters.ContextMapResolution = 4;
            behaviour.BehaviourName = "Test behaviour";

            behaviour.InstantiateContextMap(planarParameters);

            targetOne = new GameObject();
            targetOne.transform.position = new Vector3(0, 0, 5);
            targetOne.tag = "Test";
            ReferencePool.Register(targetOne);

            targetTwo = new GameObject();
            targetTwo.transform.position = new Vector3(5, 0, 0);
            targetTwo.tag = "Test";
            ReferencePool.Register(targetTwo);

            targetThree = new GameObject();
            targetThree.transform.position = new Vector3(0, 0, 0);
            targetThree.tag = "Test";
            ReferencePool.Register(targetThree);



            behaviour.Tags = new string[] { "Test" };
        }

        [TearDown]
        public void TearDown()
        {
            TagHelper.RemoveTag("Test");
            behaviour.OnDisable();

            ReferencePool.ClearAll();
        }
         
        [Test]
        public void GetPositionVectorsTest()
        {
            Vector3[] positions = TestUtilities.CallNonPublicMethod<Vector3[]>(behaviour, "getPositionVectors");
            var expected = new Vector3[] { new Vector3(0, 0, 5), new Vector3(5, 0, 0), new Vector3(0, 0, 0) };
            positions = positions.Intersect(expected).ToArray();
            Assert.AreEqual(true, positions.Contains(expected[0]));
            Assert.AreEqual(true, positions.Contains(expected[1]));
            Assert.AreEqual(true, positions.Contains(expected[2]));
        }

    }

}
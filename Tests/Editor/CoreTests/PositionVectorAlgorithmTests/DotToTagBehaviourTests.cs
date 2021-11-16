using NUnit.Framework;
using UnityEngine;
using Friedforfun.ContextSteering.Utilities;
using Friedforfun.ContextSteering.PlanarMovement;
using System.Linq;

namespace Friedforfun.ContextSteering.Tests
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
            agent.AddComponent<TagCache>();
            agent.transform.position = new Vector3(0, 0, 0);

            planarParameters.ContextMapRotationAxis = RotationAxis.YAxis;
            planarParameters.ContextMapResolution = 4;
            behaviour.BehaviourName = "Test behaviour";

            behaviour.InstantiateContextMap(planarParameters);

            targetOne = new GameObject();
            targetOne.transform.position = new Vector3(0, 0, 5);
            targetOne.tag = "Test";
            TagCache.Register(targetOne);

            targetTwo = new GameObject();
            targetTwo.transform.position = new Vector3(5, 0, 0);
            targetTwo.tag = "Test";
            TagCache.Register(targetTwo);

            targetThree = new GameObject();
            targetThree.transform.position = new Vector3(0, 0, 0);
            targetThree.tag = "Test";
            TagCache.Register(targetThree);



            behaviour.Tags = new string[] { "Test" };
        }

        [TearDown]
        public void TearDown()
        {
            TagHelper.RemoveTag("Test");
            behaviour.OnDisable();

            TagCache.ClearAll();
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
using NUnit.Framework;
using UnityEngine;
using Friedforfun.ContextSteering.Utilities;
using Friedforfun.ContextSteering.PlanarMovement;
using Unity.Collections;

namespace Friedforfun.ContextSteering.Tests
{
    public class DotToTransformTests
    {

        GameObject agent;
        GameObject targetOne;
        GameObject targetTwo;
        GameObject targetThree;

        DotToTransform behaviour;

        PlanarSteeringParameters planarParameters = new PlanarSteeringParameters();

        [SetUp]
        public void Setup()
        {
            agent = new GameObject();
            behaviour = agent.AddComponent<DotToTransform>();
            agent.transform.position = new Vector3(0, 0, 0);

            planarParameters.ContextMapRotationAxis = RotationAxis.YAxis;
            planarParameters.ContextMapResolution = 4;
            behaviour.BehaviourName = "Test behaviour";

            behaviour.InstantiateContextMap(planarParameters);

            targetOne = new GameObject();
            targetOne.transform.position = new Vector3(0, 0, 5);

            targetTwo = new GameObject();
            targetTwo.transform.position = new Vector3(5, 0, 0);

            targetThree = new GameObject();
            targetThree.transform.position = new Vector3(0, 0, 0);



            behaviour.Positions = new Transform[] { targetOne.transform, targetTwo.transform, targetThree.transform };
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
            var expected = new Vector3[] { new Vector3(0, 0, 5), new Vector3(5, 0, 0), new Vector3(0, 0, 0) };
            Assert.AreEqual(expected, positions);
        }

    }

}
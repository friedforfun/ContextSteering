using NUnit.Framework;
using UnityEngine;
using Friedforfun.ContextSteering.Core;
using Friedforfun.ContextSteering.PlanarMovement;


namespace Friedforfun.ContextSteering.Tests
{
    public class ContextCombinatorTests
    {
        ICombineContext combinator;

        [Test]
        public void BasicContextCombinator()
        {
            combinator = new BasicContextCombinator();

            // One low danger direction
            float[] steeringMap = { 1f, 0.5f, 0f, 0.5f };
            float[] maskMap = { 0.5f, 1f, 0.5f, 0.1f };
            float[] combined = combinator.CombineContext(steeringMap, maskMap);
            Assert.AreEqual(new float[] { 0, 0, 0, 0.5f }, combined);

            // two lowest danger directions
            steeringMap = new float[]{ 1f, 0.5f, 0f, 0.5f };
            maskMap = new float[]{ 0.5f, 0.1f, 0.5f, 0.1f };
            Assert.AreEqual(new float[] { 0, 0.5f, 0, 0.5f }, combinator.CombineContext(steeringMap, maskMap));

            // equal danger in all directions
            steeringMap = new float[] { 1f, 0.5f, 0f, 0.5f };
            maskMap = new float[] { 0.5f, 0.5f, 0.5f, 0.5f };
            Assert.AreEqual(steeringMap, combinator.CombineContext(steeringMap, maskMap));
        }
    }
}


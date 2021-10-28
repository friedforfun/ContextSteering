using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Friedforfun.SteeringBehaviours.Core;
using Friedforfun.SteeringBehaviours.Utilities;
using Unity.Collections;
using Unity.Jobs;

namespace Friedforfun.SteeringBehaviours.Tests
{
    public class ReferencePoolTests
    {
        GameObject go;

        [SetUp]
        public void SetUp()
        {
            ReferencePool.ClearAll();
            TagHelper.AddTag("Test");
            go = new GameObject();
            go.AddComponent<ReferencePool>();
        }

        [TearDown]
        public void TearDown()
        {
            ReferencePool.ClearAll();
            TagHelper.RemoveTag("Test");
        }

        [Test]
        public void RegisterTest()
        {
            GameObject testObject= new GameObject();
            testObject.tag = "Test";

            ReferencePool.Register(testObject);

            ReferencePool.GetGameObjectsByTag("Test");

            Assert.AreSame(testObject, ReferencePool.GetGameObjectsByTag("Test")[0]);
        }
    }
}
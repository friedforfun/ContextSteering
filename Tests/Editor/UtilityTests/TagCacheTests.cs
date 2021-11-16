using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Friedforfun.ContextSteering.Core;
using Friedforfun.ContextSteering.Utilities;
using System.Linq;

namespace Friedforfun.ContextSteering.Tests
{
    public class TagCacheTests
    {
        GameObject go;
        GameObject testObjectA;
        GameObject testObjectB;
        GameObject testObjectC;

        [SetUp]
        public void SetUp()
        {
            TagCache.ClearAll();
            TagHelper.AddTag("Test");
            TagHelper.AddTag("Dummy");
            go = new GameObject();
            go.AddComponent<TagCache>();

            testObjectA = new GameObject();
            testObjectA.tag = "Test";
            testObjectA.transform.position = new Vector3(-1, 1, 0);
            TagCache.Register(testObjectA);

            testObjectB = new GameObject();
            testObjectB.tag = "Test";
            testObjectB.transform.position = new Vector3(1, -1, 10);
            TagCache.Register(testObjectB);

            testObjectC = new GameObject();
            testObjectC.tag = "Dummy";
            testObjectC.transform.position = new Vector3(3, 0, 5);
            TagCache.Register(testObjectC);

        }

        [TearDown]
        public void TearDown()
        {
            TagCache.ClearAll();
            TagHelper.RemoveTag("Test");
            TagHelper.RemoveTag("Dummy");
        }

        [Test]
        public void RegisterTest()
        {
            Assert.AreEqual(true, TagCache.GetGameObjectsByTag("Test").Contains(testObjectA));
            Assert.AreEqual(true, TagCache.GetGameObjectsByTag("Test").Contains(testObjectB));
            Assert.AreEqual(2, TagCache.GetGameObjectsByTag("Test").Length);
        }

        [Test]
        public void DeregisterTest()
        {
            Assert.AreEqual(true, TagCache.GetGameObjectsByTag("Test").Contains(testObjectA));
            Assert.AreEqual(true, TagCache.GetGameObjectsByTag("Test").Contains(testObjectB));
            Assert.AreEqual(2, TagCache.GetGameObjectsByTag("Test").Length);

            TagCache.DeRegister(testObjectA);
            TagCache.DeRegister(testObjectB);

            Assert.AreEqual(new GameObject[] { }, TagCache.GetGameObjectsByTag("Test"));
            Assert.AreEqual(true, TagCache.GetGameObjectsByTag("Dummy").Contains(testObjectC));
            Assert.AreEqual(1, TagCache.GetGameObjectsByTag("Dummy").Length);
        }

        [Test]
        public void GetGameObjectsTest()
        {
            Assert.AreEqual(2, TagCache.GetGameObjectsByTag("Test").Length);

            GameObject[] ga = TagCache.GetGameObjectsByTag("Test");
            Assert.AreEqual(true, ga.Contains(testObjectA));
            Assert.AreEqual(true, ga.Contains(testObjectB));
            Assert.AreEqual(false, ga.Contains(testObjectC));

            Assert.AreSame(testObjectC, TagCache.GetGameObjectsByTag("Dummy")[0]);
        }

        [Test]
        public void GetVector3sTest()
        {

            Assert.AreEqual(2, TagCache.GetVector3sByTag("Test").Length);

            Vector3[] ga = TagCache.GetVector3sByTag("Test");
            Assert.AreEqual(true, ga.Contains(testObjectA.transform.position));
            Assert.AreEqual(true, ga.Contains(testObjectB.transform.position));
            Assert.AreEqual(false, ga.Contains(testObjectC.transform.position));

            Assert.AreEqual(testObjectC.transform.position, TagCache.GetVector3sByTag("Dummy")[0]);
        }

        [Test]
        public void GetCachedVector3()
        {
            Vector3[] ga = TagCache.GetVector3sByTag("Test");
            Vector3 toA = testObjectA.transform.position;
            Vector3 toB = testObjectB.transform.position;
            Vector3 toC = testObjectC.transform.position;

            Assert.AreEqual(2, ga.Length);
            Assert.AreEqual(true, ga.Contains(toA));
            Assert.AreEqual(true, ga.Contains(toB));
            Assert.AreEqual(false, ga.Contains(toC));
            Assert.AreEqual(testObjectC.transform.position, TagCache.GetVector3sByTag("Dummy")[0]);

            testObjectA.transform.position = new Vector3(10, 123, 5);
            testObjectB.transform.position = new Vector3(13, 13, 1);
            testObjectC.transform.position = new Vector3(-10, 3, 0);

            Vector3[] gaAfter = TagCache.GetVector3sByTag("Test");
            Assert.AreEqual(2, ga.Length);
            Assert.AreEqual(true, gaAfter.Contains(toA));
            Assert.AreEqual(true, gaAfter.Contains(toB));
            Assert.AreEqual(false, gaAfter.Contains(testObjectA.transform.position));
            Assert.AreEqual(false, gaAfter.Contains(testObjectB.transform.position));
            Assert.AreEqual(false, gaAfter.Contains(testObjectC.transform.position));

        }

        [Test]
        public void ClearPositionCache()
        {
            Vector3[] ga = TagCache.GetVector3sByTag("Test");
            Vector3 toA = testObjectA.transform.position;
            Vector3 toB = testObjectB.transform.position;
            Vector3 toC = testObjectC.transform.position;

            Assert.AreEqual(2, ga.Length);
            Assert.AreEqual(true, ga.Contains(toA));
            Assert.AreEqual(true, ga.Contains(toB));
            Assert.AreEqual(false, ga.Contains(toC));
            Assert.AreEqual(testObjectC.transform.position, TagCache.GetVector3sByTag("Dummy")[0]);

            testObjectA.transform.position = new Vector3(10, 123, 5);
            testObjectB.transform.position = new Vector3(13, 13, 1);
            testObjectC.transform.position = new Vector3(-10, 3, 0);

            TagCache.ClearCache();

            Vector3[] gaAfter = TagCache.GetVector3sByTag("Test");
            Assert.AreEqual(2, gaAfter.Length);
            Assert.AreEqual(false, gaAfter.Contains(toA));
            Assert.AreEqual(false, gaAfter.Contains(toB));
            Assert.AreEqual(true, gaAfter.Contains(testObjectA.transform.position));
            Assert.AreEqual(true, gaAfter.Contains(testObjectB.transform.position));
            Assert.AreEqual(false, gaAfter.Contains(testObjectC.transform.position));
        }

        [Test]
        public void ClearAllTest()
        {
            Vector3[] ga = TagCache.GetVector3sByTag("Test");
            Vector3 toA = testObjectA.transform.position;
            Vector3 toB = testObjectB.transform.position;
            Vector3 toC = testObjectC.transform.position;

            Assert.AreEqual(2, ga.Length);
            Assert.AreEqual(true, ga.Contains(toA));
            Assert.AreEqual(true, ga.Contains(toB));
            Assert.AreEqual(false, ga.Contains(toC));
            Assert.AreEqual(testObjectC.transform.position, TagCache.GetVector3sByTag("Dummy")[0]);

            testObjectA.transform.position = new Vector3(10, 123, 5);
            testObjectB.transform.position = new Vector3(13, 13, 1);
            testObjectC.transform.position = new Vector3(-10, 3, 0);

            TagCache.ClearAll();

            Vector3[] gaAfter = TagCache.GetVector3sByTag("Test");
            Assert.AreEqual(0, gaAfter.Length);
            Assert.AreEqual(false, gaAfter.Contains(toA));
            Assert.AreEqual(false, gaAfter.Contains(toB));
            Assert.AreEqual(false, gaAfter.Contains(testObjectA.transform.position));
            Assert.AreEqual(false, gaAfter.Contains(testObjectB.transform.position));
            Assert.AreEqual(false, gaAfter.Contains(testObjectC.transform.position));
        }
    }
}
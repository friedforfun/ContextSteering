using NUnit.Framework;
using Friedforfun.ContextSteering.PlanarMovement;
using System;

namespace Friedforfun.ContextSteering.Tests
{
    public class PlanarSteeringParameterTests
    {
        PlanarSteeringParameters planarParams;

        [SetUp]
        public void Setup()
        {
            planarParams = new PlanarSteeringParameters();
        }

        [Test]
        public void ChangeResolutionTest()
        {
            Assert.AreEqual(12, planarParams.ContextMapResolution);

            planarParams.ContextMapResolution = 24;

            Assert.AreEqual(24, planarParams.ContextMapResolution);

        }

        //[Test]
        public void OnChangeResolutionEventTest()
        {
            bool eventFired = false;

            //planarParams.OnResolutionChange += delegate (object sender, EventArgs e) { eventFired = true; };

            planarParams.ContextMapResolution = 24;

            Assert.AreEqual(true, eventFired);
        }


        [Test]
        public void ResolutionAngleTest()
        {

            Assert.AreEqual(360 / 12f, planarParams.ResolutionAngle);

            planarParams.ContextMapResolution = 24;

            Assert.AreEqual(360 / 24f, planarParams.ResolutionAngle);
        }
    }
}
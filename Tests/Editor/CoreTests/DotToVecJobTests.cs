using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Friedforfun.ContextSteering.Core;
using Friedforfun.ContextSteering.Utilities;
using Unity.Collections;
using Unity.Jobs;

namespace Friedforfun.ContextSteering.Tests
{
    public class DotToVecJobTests
    {

        Vector3[] targets = { new Vector3(0, 0, 10) };

        bool scaled = false;
        Vector3 position = new Vector3(0, 0, 0);
        float range = 11;
        float weight = 1;
        float angle = 90;
        float invertScale = 0;

        RotationAxis axis = RotationAxis.YAxis;

        float[] _weights = { 0f, 0f, 0f, 0f };

        SteerDirection direction = SteerDirection.ATTRACT;

        [SetUp]
        public void Setup()
        {
            targets = new Vector3[] { new Vector3(0, 0, 10) };

            scaled = false;
            position = new Vector3(0, 0, 0);
            range = 11;
            weight = 1;
            angle = 90;
            invertScale = 0;

            axis = RotationAxis.YAxis;

            _weights = new float[] { 0f, 0f, 0f, 0f };

            direction = SteerDirection.ATTRACT;
        }

        [Test]
        public void StandardMapYAxis()
        {
            NativeArray<Vector3> targetPositions = new NativeArray<Vector3>(targets, Allocator.Temp);
            NativeArray<float> Weights = new NativeArray<float>(_weights, Allocator.Temp);

            DotToVecJob job = new DotToVecJob()
            {
                targets = targetPositions,
                scaled = scaled,
                my_position = position,
                range = range,
                weight = weight,
                angle = angle,
                invertScale = invertScale,
                axis = axis,
                Weights = Weights,
                direction = direction
            };

            // compute
            job.Execute();

            // check result
            float[] expected = { 1f, 0f, -1f, 0f };

            Assert.AreEqual(expected[0], Weights[0], TestUtilities.DOTPRODTOLERANCE);
            Assert.AreEqual(expected[1], Weights[1], TestUtilities.DOTPRODTOLERANCE);
            Assert.AreEqual(expected[2], Weights[2], TestUtilities.DOTPRODTOLERANCE);
            Assert.AreEqual(expected[3], Weights[3], TestUtilities.DOTPRODTOLERANCE);

            targetPositions.Dispose();
            Weights.Dispose();

        }

        [Test]
        public void StandardMapYAxisScheduled()
        {
            NativeArray<Vector3> targetPositions = new NativeArray<Vector3>(targets, Allocator.TempJob);
            NativeArray<float> Weights = new NativeArray<float>(_weights, Allocator.TempJob);

            DotToVecJob job = new DotToVecJob()
            {
                targets = targetPositions,
                scaled = scaled,
                my_position = position,
                range = range,
                weight = weight,
                angle = angle,
                invertScale = invertScale,
                axis = axis,
                Weights = Weights,
                direction = direction
            };

            // compute
            JobHandle handle = job.Schedule();

            handle.Complete();

            // check result
            float[] expected = { 1f, 0f, -1f, 0f };

            Assert.AreEqual(expected[0], Weights[0], TestUtilities.DOTPRODTOLERANCE);
            Assert.AreEqual(expected[1], Weights[1], TestUtilities.DOTPRODTOLERANCE);
            Assert.AreEqual(expected[2], Weights[2], TestUtilities.DOTPRODTOLERANCE);
            Assert.AreEqual(expected[3], Weights[3], TestUtilities.DOTPRODTOLERANCE);

            targetPositions.Dispose();
            Weights.Dispose();

        }

        [Test]
        public void StandardYAxisTargetAtZero()
        {
            targets = new Vector3[] { new Vector3(0, 0, 0) };
            NativeArray<Vector3> targetPositions = new NativeArray<Vector3>(targets, Allocator.Temp);
            NativeArray<float> Weights = new NativeArray<float>(_weights, Allocator.Temp);

            DotToVecJob job = new DotToVecJob()
            {
                targets = targetPositions,
                scaled = scaled,
                my_position = position,
                range = range,
                weight = weight,
                angle = angle,
                invertScale = invertScale,
                axis = axis,
                Weights = Weights,
                direction = direction
            };

            // compute
            job.Execute();

            // check result
            float[] expected = { 0f, 0f, 0f, 0f };

            Assert.AreEqual(expected[0], Weights[0], TestUtilities.DOTPRODTOLERANCE);
            Assert.AreEqual(expected[1], Weights[1], TestUtilities.DOTPRODTOLERANCE);
            Assert.AreEqual(expected[2], Weights[2], TestUtilities.DOTPRODTOLERANCE);
            Assert.AreEqual(expected[3], Weights[3], TestUtilities.DOTPRODTOLERANCE);

            targetPositions.Dispose();
            Weights.Dispose();
        }

        [Test]
        public void WeightedYAxis()
        {
            weight = 0.2f;

            NativeArray<Vector3> targetPositions = new NativeArray<Vector3>(targets, Allocator.Temp);
            NativeArray<float> Weights = new NativeArray<float>(_weights, Allocator.Temp);

            DotToVecJob job = new DotToVecJob()
            {
                targets = targetPositions,
                scaled = scaled,
                my_position = position,
                range = range,
                weight = weight,
                angle = angle,
                invertScale = invertScale,
                axis = axis,
                Weights = Weights,
                direction = direction
            };

            // compute
            job.Execute();

            // check result
            float[] expected = { 0.2f, 0f, -0.2f, 0f };

            Assert.AreEqual(expected[0], Weights[0], TestUtilities.DOTPRODTOLERANCE);
            Assert.AreEqual(expected[1], Weights[1], TestUtilities.DOTPRODTOLERANCE);
            Assert.AreEqual(expected[2], Weights[2], TestUtilities.DOTPRODTOLERANCE);
            Assert.AreEqual(expected[3], Weights[3], TestUtilities.DOTPRODTOLERANCE);

            targetPositions.Dispose();
            Weights.Dispose();

        }

        [Test]
        public void RangeYAxis()
        {

            // ------------------- Out of Range -------------------
            range = 10;
            targets = new Vector3[] { new Vector3(0, 0, 15) };

            NativeArray<Vector3> targetPositions = new NativeArray<Vector3>(targets, Allocator.Temp);
            NativeArray<float> Weights = new NativeArray<float>(_weights, Allocator.Temp);

            DotToVecJob job = new DotToVecJob()
            {
                targets = targetPositions,
                scaled = scaled,
                my_position = position,
                range = range,
                weight = weight,
                angle = angle,
                invertScale = invertScale,
                axis = axis,
                Weights = Weights,
                direction = direction
            };

            // compute
            job.Execute();

            // check result
            float[] expected = { 0f, 0f, 0f, 0f };

            Assert.AreEqual(expected[0], Weights[0], TestUtilities.DOTPRODTOLERANCE);
            Assert.AreEqual(expected[1], Weights[1], TestUtilities.DOTPRODTOLERANCE);
            Assert.AreEqual(expected[2], Weights[2], TestUtilities.DOTPRODTOLERANCE);
            Assert.AreEqual(expected[3], Weights[3], TestUtilities.DOTPRODTOLERANCE);

            targetPositions.Dispose();

            // ------------------- In Range -------------------
            targets = new Vector3[] { new Vector3(0, 0, 8) };

            NativeArray<Vector3> newTargetPositions = new NativeArray<Vector3>(targets, Allocator.Temp);

            job = new DotToVecJob()
            {
                targets = newTargetPositions,
                scaled = scaled,
                my_position = position,
                range = range,
                weight = weight,
                angle = angle,
                invertScale = invertScale,
                axis = axis,
                Weights = Weights,
                direction = direction
            };

            // compute
            job.Execute();

            // check result
            expected = new float[] { 1f, 0f, -1f, 0f };


            //Assert.AreEqual(expected[0], Weights[0], TestUtilities.DOTPRODTOLERANCE));
            Assert.AreEqual(expected[0], Weights[0]);
            Assert.AreEqual(expected[1], Weights[1], TestUtilities.DOTPRODTOLERANCE);
            Assert.AreEqual(expected[2], Weights[2], TestUtilities.DOTPRODTOLERANCE);
            Assert.AreEqual(expected[3], Weights[3], TestUtilities.DOTPRODTOLERANCE);

            newTargetPositions.Dispose();

            // ------------------- Same Position -------------------
            targets = new Vector3[] { new Vector3(0, 0, 0) };
            NativeArray<Vector3> targetOnMe = new NativeArray<Vector3>(targets, Allocator.Temp);

            job = new DotToVecJob()
            {
                targets = targetOnMe,
                scaled = scaled,
                my_position = position,
                range = range,
                weight = weight,
                angle = angle,
                invertScale = invertScale,
                axis = axis,
                Weights = Weights,
                direction = direction
            };

            job.Execute();
            
            expected = new float[] { 0, 0, 0, 0 };
            Assert.AreEqual(expected[0], Weights[0], TestUtilities.DOTPRODTOLERANCE);
            Assert.AreEqual(expected[1], Weights[1], TestUtilities.DOTPRODTOLERANCE);
            Assert.AreEqual(expected[2], Weights[2], TestUtilities.DOTPRODTOLERANCE);
            Assert.AreEqual(expected[3], Weights[3], TestUtilities.DOTPRODTOLERANCE);

            targetOnMe.Dispose();
            Weights.Dispose();
        }

        [Test]
        public void ScaledMapYAxis()
        {
            scaled = true;
            range = 20;

            NativeArray<Vector3> targetPositions = new NativeArray<Vector3>(targets, Allocator.Temp);
            NativeArray<float> Weights = new NativeArray<float>(_weights, Allocator.Temp);

            DotToVecJob job = new DotToVecJob()
            {
                targets = targetPositions,
                scaled = scaled,
                my_position = position,
                range = range,
                weight = weight,
                angle = angle,
                invertScale = invertScale,
                axis = axis,
                Weights = Weights,
                direction = direction
            };

            // compute
            job.Execute();

            // check result
            float[] expected = { 0.5f, 0f, -0.5f, 0f };

            Assert.AreEqual(expected[0], Weights[0], TestUtilities.DOTPRODTOLERANCE);
            Assert.AreEqual(expected[1], Weights[1], TestUtilities.DOTPRODTOLERANCE);
            Assert.AreEqual(expected[2], Weights[2], TestUtilities.DOTPRODTOLERANCE);
            Assert.AreEqual(expected[3], Weights[3], TestUtilities.DOTPRODTOLERANCE);

            targetPositions.Dispose();
            Weights.Dispose();
        }

        [Test]
        public void InverseScaledMapYAxis()
        {
            range = 20;
            targets = new Vector3[] { new Vector3(0, 0, 15) };
            scaled = true;
            invertScale = 1;

            NativeArray<Vector3> targetPositions = new NativeArray<Vector3>(targets, Allocator.Temp);

            NativeArray<float> Weights = new NativeArray<float>(_weights, Allocator.Temp);

            DotToVecJob job = new DotToVecJob()
            {
                targets = targetPositions,
                scaled = scaled,
                my_position = position,
                range = range,
                weight = weight,
                angle = angle,
                invertScale = invertScale,
                axis = axis,
                Weights = Weights,
                direction = direction
            };

            // compute
            job.Execute();

            // check result
            float[] expected = { 0.25f, 0f, -0.25f, 0f };

            Assert.AreEqual(expected[0], Weights[0], TestUtilities.DOTPRODTOLERANCE);
            Assert.AreEqual(expected[1], Weights[1], TestUtilities.DOTPRODTOLERANCE);
            Assert.AreEqual(expected[2], Weights[2], TestUtilities.DOTPRODTOLERANCE);
            Assert.AreEqual(expected[3], Weights[3], TestUtilities.DOTPRODTOLERANCE);

            targetPositions.Dispose();
            Weights.Dispose();
        }

        [Test]
        public void ZAxis()
        {
            axis = RotationAxis.ZAxis;
            targets = new Vector3[] { new Vector3(0, 10, 0) };

            NativeArray<Vector3> targetPositions = new NativeArray<Vector3>(targets, Allocator.Temp);
            NativeArray<float> Weights = new NativeArray<float>(_weights, Allocator.Temp);

            SteerDirection direction = SteerDirection.ATTRACT;

            DotToVecJob job = new DotToVecJob()
            {
                targets = targetPositions,
                scaled = scaled,
                my_position = position,
                range = range,
                weight = weight,
                angle = angle,
                invertScale = invertScale,
                axis = axis,
                Weights = Weights,
                direction = direction
            };

            // compute
            job.Execute();

            // check result
            float[] expected = { 1f, 0f, -1f, 0f };

            Assert.AreEqual(expected[0], Weights[0], TestUtilities.DOTPRODTOLERANCE);
            Assert.AreEqual(expected[1], Weights[1], TestUtilities.DOTPRODTOLERANCE);
            Assert.AreEqual(expected[2], Weights[2], TestUtilities.DOTPRODTOLERANCE);
            Assert.AreEqual(expected[3], Weights[3], TestUtilities.DOTPRODTOLERANCE);

            targetPositions.Dispose();
            Weights.Dispose();
        }

        [Test]
        public void XAxis()
        {
            axis = RotationAxis.XAxis;

            NativeArray<Vector3> targetPositions = new NativeArray<Vector3>(targets, Allocator.Temp);
            NativeArray<float> Weights = new NativeArray<float>(_weights, Allocator.Temp);

            DotToVecJob job = new DotToVecJob()
            {
                targets = targetPositions,
                scaled = scaled,
                my_position = position,
                range = range,
                weight = weight,
                angle = angle,
                invertScale = invertScale,
                axis = axis,
                Weights = Weights,
                direction = direction
            };

            // compute
            job.Execute();

            // check result
            float[] expected = { 1f, 0f, -1f, 0f };

            Assert.AreEqual(expected[0], Weights[0], TestUtilities.DOTPRODTOLERANCE);
            Assert.AreEqual(expected[1], Weights[1], TestUtilities.DOTPRODTOLERANCE);
            Assert.AreEqual(expected[2], Weights[2], TestUtilities.DOTPRODTOLERANCE);
            Assert.AreEqual(expected[3], Weights[3], TestUtilities.DOTPRODTOLERANCE);

            targetPositions.Dispose();
            Weights.Dispose();

        }

        [Test]
        public void TargetPositionsDisposed()
        {
            NativeArray<Vector3> targetPositions = new NativeArray<Vector3>(targets, Allocator.Temp);
            NativeArray<float> Weights = new NativeArray<float>(_weights, Allocator.Temp);

            DotToVecJob job = new DotToVecJob()
            {
                targets = targetPositions,
                scaled = scaled,
                my_position = position,
                range = range,
                weight = weight,
                angle = angle,
                invertScale = invertScale,
                axis = axis,
                Weights = Weights,
                direction = direction
            };

            // compute
            job.Execute();

            targetPositions.Dispose();
            // check result
            Assert.AreEqual(false, targetPositions.IsCreated);

            Weights.Dispose();
        }


    }
}
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Friedforfun.SteeringBehaviours.Core;
using Friedforfun.SteeringBehaviours.Utilities;
using Unity.Collections;

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

    void SetDefaults()
    {
        targets = new Vector3[]{ new Vector3(0, 0, 10) };

        scaled = false;
        position = new Vector3(0, 0, 0);
        range = 11;
        weight = 1;
        angle = 90;
        invertScale = 0;

        axis = RotationAxis.YAxis;

        _weights = new float[]{ 0f, 0f, 0f, 0f };

        direction = SteerDirection.ATTRACT;
    }

    [Test]
    public void StandardMapYAxis()
    {
        // create a very simple job
        SetDefaults();
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

        float tolerance = 0.000001f;

        Assert.AreEqual(true, CmpTol(expected[0], Weights[0], tolerance));
        Assert.AreEqual(true, CmpTol(expected[1], Weights[1], tolerance));
        Assert.AreEqual(true, CmpTol(expected[2], Weights[2], tolerance));
        Assert.AreEqual(true, CmpTol(expected[3], Weights[3], tolerance));

        targetPositions.Dispose();
        Weights.Dispose();

    }

    [Test]
    public void ScaledMapYAxis()
    {
        SetDefaults();
        // create a very simple job

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

        float tolerance = 0.000001f;

        Assert.AreEqual(true, CmpTol(expected[0], Weights[0], tolerance));
        Assert.AreEqual(true, CmpTol(expected[1], Weights[1], tolerance));
        Assert.AreEqual(true, CmpTol(expected[2], Weights[2], tolerance));
        Assert.AreEqual(true, CmpTol(expected[3], Weights[3], tolerance));

        targetPositions.Dispose();
        Weights.Dispose();
    }

    [Test]
    public void InverseScaledMapYAxis()
    {
        SetDefaults();
        // create a very simple job

        range = 20;
        targets = new Vector3[]{ new Vector3(0, 0, 15) };
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

        float tolerance = 0.000001f;

        Assert.AreEqual(true, CmpTol(expected[0], Weights[0], tolerance));
        Assert.AreEqual(true, CmpTol(expected[1], Weights[1], tolerance));
        Assert.AreEqual(true, CmpTol(expected[2], Weights[2], tolerance));
        Assert.AreEqual(true, CmpTol(expected[3], Weights[3], tolerance));

        targetPositions.Dispose();
        Weights.Dispose();
    }

    [Test]
    public void ZAxis()
    {
        SetDefaults();
        // create a very simple job
        axis = RotationAxis.ZAxis;
        targets = new Vector3[]{ new Vector3(0, 10, 0) };

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

        float tolerance = 0.000001f;

        Assert.AreEqual(true, CmpTol(expected[0], Weights[0], tolerance));
        Assert.AreEqual(true, CmpTol(expected[1], Weights[1], tolerance));
        Assert.AreEqual(true, CmpTol(expected[2], Weights[2], tolerance));
        Assert.AreEqual(true, CmpTol(expected[3], Weights[3], tolerance));

        targetPositions.Dispose();
        Weights.Dispose();
    }

    [Test]
    public void XAxis()
    {
        SetDefaults();
        // create a very simple job
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

        float tolerance = 0.000001f;

        Assert.AreEqual(true, CmpTol(expected[0], Weights[0], tolerance));
        Assert.AreEqual(true, CmpTol(expected[1], Weights[1], tolerance));
        Assert.AreEqual(true, CmpTol(expected[2], Weights[2], tolerance));
        Assert.AreEqual(true, CmpTol(expected[3], Weights[3], tolerance));

        targetPositions.Dispose();
        Weights.Dispose();

    }

    [Test]
    public void TargetPositionsDisposed()
    {
        SetDefaults();
        // create a very simple job

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

    private bool CmpTol(float a, float b, float tol)
    {
        return Mathf.Abs(a - b) < tol;
    }
}
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

//public struct PullData
//{
//    public float3[] Positions;
//    public float[] Strengths;

//    public int Length;

//    public PullData(int length)
//    {
//        Length = length;
//        Positions = new float3[Length];
//        Strengths = new float[Length];
//    }
//}

//public struct PullData
//{
//    public float3 Position;
//    public float Strength;
//}

public struct PullData
{
    public NativeArray<float3> Position;
    public NativeArray<float> Strength;
    public int Count;

    public PullData(int count)
    {
        Count = count;
        Position = new NativeArray<float3>(new float3[Count], Allocator.TempJob);
        Strength = new NativeArray<float>(new float[Count], Allocator.TempJob);
    }
}
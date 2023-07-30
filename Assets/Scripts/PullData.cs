using System.Collections;
using System.Collections.Generic;
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

public struct PullData
{
    public float3 Position;
    public float Strength;
}

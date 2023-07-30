// Ignore Spelling: Pullable

using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public struct Pullable : IComponentData
{
    public float PullStrength;
    public float3 PullCenter;
}

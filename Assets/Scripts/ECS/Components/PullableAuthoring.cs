// Ignore Spelling: Pullable

using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class PullableAuthoring : MonoBehaviour
{
    public float PullStrength;
    public float3 PullCenter;
}


public class PullableBaker : Baker<PullableAuthoring>
{
    public override void Bake(PullableAuthoring authoring)
    {
        //TransformUsageFlags transformUsageFlags = new TransformUsageFlags();
        Entity entity = GetEntity(authoring, TransformUsageFlags.Dynamic); //this.GetEntity(transformUsageFlags /*TransformUsageFlags.Dynamic*/);
        AddComponent(entity, new Pullable
        {
            PullStrength = authoring.PullStrength,
            PullCenter = authoring.PullCenter
        });
        
    }
}

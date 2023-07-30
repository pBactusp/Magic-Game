// Ignore Spelling: Pullable

using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Physics.Aspects;
using UnityEngine;
using Unity.Collections;

public partial class PullableSystemBase : SystemBase
{
    //private PullData[] pd;
    protected override void OnUpdate()
    {
        //pd = GravitySpellsManager.GetPullData();
        NativeArray<PullData> pd = new NativeArray<PullData>(GravitySpellsManager.GetPullData(), Allocator.TempJob);
        float deltaTime = SystemAPI.Time.DeltaTime;

        Entities.ForEach((PullableAspect pullable) =>
        {
            pullable.BePulled(pd, deltaTime);
        }).Schedule();

        //Entities.ForEach((RigidBodyAspect rb, in Pullable pullable) =>
        //{
        //    var dir = pullable.PullCenter - rb.Position;
        //    var force = math.normalize(dir) * pullable.PullStrength / math.max(math.length(dir), 1);
        //    rb.ApplyLinearImpulseWorldSpace(force);
        //}).Schedule();

        //foreach (LocalToWorld ltw in SystemAPI.Query<LocalToWorld>())
        //{
        //    ltw.Value[3] += new float4(1f, 0f, 0f, 0f);
        //}
    }
}

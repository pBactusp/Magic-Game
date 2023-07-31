// Ignore Spelling: pullable

using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;
using UnityEngine.Scripting;

public partial struct PullableISystem : ISystem
{
    void OnCreate(ref SystemState state) { }

    void OnDestroy(ref SystemState state) { }

    void OnUpdate(ref SystemState state)
    {
        //NativeArray<PullData> pd = new NativeArray<PullData>(GravitySpellsManager.GetPullData(), Allocator.TempJob);
        var pd = GravitySpellsManager.GetPullData();
        float deltaTime = SystemAPI.Time.DeltaTime;

        //Entities.ForEach((PullableAspect pullable) =>
        //{
        //    pullable.BePulled(pd, deltaTime);
        //}).Schedule();

        foreach (PullableAspect pullable in SystemAPI.Query<PullableAspect>())
        {
            pullable.BePulled(pd, deltaTime);
        }

        //new PullJob()
        //{
        //    PullData = pd,
        //    DeltaTime = deltaTime,
        //}.ScheduleParallel();
    }
}

public partial struct PullJob : IJobEntity
{
    public PullData PullData;
    public float DeltaTime;

    public void Execute(PullableAspect pullable)
    {
        pullable.BePulled(PullData, DeltaTime);
    }
}
// Ignore Spelling: Pullable

using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Entities.UniversalDelegates;
using Unity.Mathematics;
using Unity.Physics.Aspects;
using UnityEngine;

public readonly partial struct PullableAspect : IAspect
{
    private readonly Entity entity;

    private readonly RigidBodyAspect rb;
    private readonly RefRO<Pullable> pullable;

    public void BePulled(PullData pullData, float deltaTime)
    {
        float3 force = Vector3.zero;

        //for (int i = 0; i < pullData.Length; i++)
        //{
        //    var dir = pullData.Positions[i] - rb.Position;
        //    force += math.normalize(dir) * pullData.Strengths[i] / math.max(math.length(dir), 1);
        //}

        //for (int i = 0; i < pullData.Length; i++)
        //{
        //    var dir = pullData[i].Position - rb.Position;
        //    force += math.normalize(dir) * pullData[i].Strength / math.max(math.length(dir), 1);
        //}

        //foreach (var data in pullData)
        for (int i = 0; i < pullData.Count; i++)
        {
            var dir = pullData.Position[i] - rb.Position;
            force += math.normalize(dir) * pullData.Strength[i] / math.max(math.length(dir), 1);
        }

        rb.ApplyLinearImpulseWorldSpace(force * deltaTime);
    }
}

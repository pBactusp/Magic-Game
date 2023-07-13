using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public abstract class BaseProjectile : Spell
{
    private float speed;
    private Vector3 direction;
    private Transform target;

    protected BaseProjectile(Transform origin, Vector3 direction, Transform target) : base(origin)
    {
        this.direction = direction;
        this.target = target;
    }
}

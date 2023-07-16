using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public abstract class BaseProjectile : Spell
{
    [SerializeField] private float speed;
    private Vector3 direction;
    private Transform target;


    public void Init(Transform origin, Vector3 direction, Transform target)
    {
        base.Init(origin);

        this.direction = direction;
        this.target = target;
    }
}

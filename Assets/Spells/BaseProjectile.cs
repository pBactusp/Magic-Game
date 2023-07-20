using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public abstract class BaseProjectile : Spell
{
    [SerializeField] protected float gravity;
    [SerializeField] protected float speed;
    protected Rigidbody rb;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void Start()
    {
        rb.useGravity = (gravity != 0);
    }

    protected override void OnLaunch()
    {
        transform.parent = null;
    }

}

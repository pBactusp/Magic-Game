using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public abstract class BaseProjectile : Spell
{
    [SerializeField] protected float gravity;
    [SerializeField] protected float speed;
    [SerializeField] protected AnimationCurve speedOverLifeTime;

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
        StartCoroutine(ChangeSpeedOverLifeTime());
    }

    protected override void Die()
    {
        base.Die();
    }


    private IEnumerator ChangeSpeedOverLifeTime()
    {
        while (isAlive)
        {
            var normalizedVelocity = rb.velocity.normalized;
            rb.velocity = normalizedVelocity * Mathf.Lerp(0f, speed, speedOverLifeTime.Evaluate(normalizedLifetime));
            yield return null;
        }
    }
}

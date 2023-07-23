using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushProjectile : BasePushSpell
{
    private Vector3 initialScale;
    protected override void OnSpawn()
    {
        base.OnSpawn();
        initialScale = transform.localScale;
        transform.localScale = Vector3.zero;
    }

    protected override void OnLaunch()
    {
        transform.localScale = initialScale;
        base.OnLaunch();
    }

    private void OnTriggerEnter(Collider other)
    {
        Push(rb.velocity);
        Die();
    }
}

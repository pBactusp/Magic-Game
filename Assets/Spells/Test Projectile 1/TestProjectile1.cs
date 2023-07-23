using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestProjectile1 : BaseProjectile
{
    protected override void OnSpawn()
    {
        base.OnSpawn();
        gameObject.SetActive(false);
    }
    protected override void OnLaunch()
    {
        gameObject.SetActive(true);
        base.OnLaunch();
        rb.velocity = args.Direction * speed;
    }
}

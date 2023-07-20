using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestProjectile1 : BaseProjectile
{
    protected override void OnLaunch()
    {
        base.OnLaunch();
        rb.velocity = args.Direction * speed;
    }
}

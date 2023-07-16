using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestProjectile1 : BaseProjectile
{
    protected override void Behavior()
    {
        rb.velocity = args.Direction * speed;
    }
}

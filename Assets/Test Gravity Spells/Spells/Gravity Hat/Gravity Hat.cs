using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityHat : BaseGravitySpell
{
    [SerializeField] protected Vector3 offset;

    protected override void OnLaunch()
    {
        base.OnLaunch();
        transform.position += offset;
    }
}

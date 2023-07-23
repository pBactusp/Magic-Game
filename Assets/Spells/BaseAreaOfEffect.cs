using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAreaOfEffect : BaseProjectile
{
    protected override void OnLaunch()
    {
        StartCoroutine(ChangeSpeedOverLifeTime());
    }
}

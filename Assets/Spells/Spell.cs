using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public abstract class Spell : MonoBehaviour
{
    protected Transform origin;
    [SerializeField] protected DamageData damage;

    public Action OnHit;

    public virtual void Init(Transform origin)
    {
        this.origin = origin;
    }
}

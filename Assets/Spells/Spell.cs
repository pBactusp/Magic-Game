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

    protected Spell(Transform origin)
    {
        this.origin = origin;
    }
}

//Spell:
//{
//    Fields:
//    {
//        Origin,
//    Damage,
//  },
//  Events:
//    {
//        OnHit
//  }
//}

//Projectile(Spell):
//{
//    Fields:
//    {
//        Direction,
//    Target,

//  },
//  Events:
//    {

//    }
//}
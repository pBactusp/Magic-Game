using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public abstract class Spell : MonoBehaviour
{
    protected SpellInitializationArguments args;
    [SerializeField] protected float lifeTime;
    [SerializeField] protected DamageData damage;

    public Action OnHit;

    public virtual void Init(SpellInitializationArguments args)
    {
        this.args = args;

        transform.position = args.Origin.position;
        transform.rotation = args.Origin.rotation;
        transform.parent = args.Parent;
        
        Behavior();
    }

    protected abstract void Behavior();
    protected virtual void Die()
    {
        gameObject.SetActive(false);
        Destroy(gameObject);
    }
}

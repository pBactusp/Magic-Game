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

    protected float normalizedLifetime { get; private set; }

    public void Init(SpellInitializationArguments args)
    {
        this.args = args;

        transform.position = args.Origin.position;
        transform.rotation = args.Origin.rotation;
        transform.parent = args.Parent;

        normalizedLifetime = 0;

        StartCoroutine(WaitForDeath());
        Behavior();
    }

    public void Spawn(Transform parent)
    {
        transform.parent = parent;
        transform.localPosition = Vector3.zero;
    }
    public void Launch(SpellInitializationArguments args)
    {
        this.args = args;

        transform.position = args.Origin.position;
        transform.rotation = args.Origin.rotation;
        transform.parent = args.Parent;

        normalizedLifetime = 0;

        StartCoroutine(WaitForDeath());
        Behavior();
    }

    IEnumerator WaitForDeath()
    {
        float elapsed = 0;

        while (elapsed < lifeTime)
        {
            normalizedLifetime = elapsed / lifeTime;
            elapsed += Time.deltaTime;
            yield return null;
        }

        Die();
    }

    protected abstract void Behavior();
    protected virtual void Die()
    {
        gameObject.SetActive(false);
        Destroy(gameObject);
    }
}

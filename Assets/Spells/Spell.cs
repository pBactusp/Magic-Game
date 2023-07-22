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
    protected bool isAlive { get; private set; }


    public void Spawn(Transform parent)
    {
        transform.parent = parent;
        transform.localPosition = Vector3.zero;

        OnSpawn();
    }
    public void Launch(SpellInitializationArguments args)
    {
        this.args = args;

        transform.position = args.Origin.position;
        transform.rotation = args.Origin.rotation;
        transform.parent = args.Parent;

        normalizedLifetime = 0;
        isAlive = true;

        StartCoroutine(WaitForDeath());
        OnLaunch();
        OnLateLaunch();
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

    protected virtual void OnSpawn() { }
    protected virtual void OnLaunch() { }
    protected virtual void OnLateLaunch() { }
    protected virtual void Die()
    {
        isAlive = false;
        gameObject.SetActive(false);
        Destroy(gameObject);
    }
}


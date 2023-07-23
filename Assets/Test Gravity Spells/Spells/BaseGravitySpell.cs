using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseGravitySpell : BaseAreaOfEffect
{
    [SerializeField] protected float pullForce;
    [SerializeField] protected float pullRadius;
    [SerializeField] protected bool falloff;

    [Header("Performance")]
    [SerializeField] protected float updatePullAblesDelay;

    private Rigidbody[] pullAbleObjects;

    protected override void OnLaunch()
    {
        base.OnLaunch();
        rb.velocity = args.Direction * speed;
        StartCoroutine(UpdatePullAbles());
        StartCoroutine(Pull());
    }

    protected override void Die()
    {
        base.Die();
    }

    private IEnumerator UpdatePullAbles()
    {
        var wait = new WaitForSeconds(updatePullAblesDelay);

        while (isAlive)
        {
            var cols = Physics.OverlapSphere(transform.position, pullRadius, LayerMasks.PullAble);
            pullAbleObjects = new Rigidbody[cols.Length];

            for (int i = 0; i < cols.Length; i++)
                pullAbleObjects[i] = cols[i].GetComponent<Rigidbody>();

            yield return wait;
        }

    }

    private IEnumerator Pull()
    {
        while (isAlive)
        {
            for (int i = 0; i < pullAbleObjects.Length; i++)
            {
                var pullVector = transform.position - pullAbleObjects[i].transform.position;

                if (falloff)
                    pullAbleObjects[i].AddForce(pullVector.normalized * pullForce);
                else
                    pullAbleObjects[i].AddForce(pullVector.normalized * pullForce / pullVector.sqrMagnitude);

            }

            yield return null;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, pullRadius);
    }
}

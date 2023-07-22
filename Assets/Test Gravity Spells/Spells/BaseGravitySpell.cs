using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseGravitySpell : BaseAreaOfEffect
{
    [SerializeField] protected float pullForce;
    [SerializeField] protected float pullRadius;

    [Header("Performance")]
    [SerializeField] protected float updatePullAblesDelay;

    private PullAble[] pullAbleObjects;

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
            pullAbleObjects = new PullAble[cols.Length];

            for (int i = 0; i < cols.Length; i++)
                pullAbleObjects[i] = cols[i].GetComponent<PullAble>();

            yield return wait;
        }

    }

    private IEnumerator Pull()
    {
        while (isAlive)
        {
            for (int i = 0; i < pullAbleObjects.Length; i++)
            {
                var pullDirection = (transform.position - pullAbleObjects[i].transform.position).normalized;
                pullAbleObjects[i].ApplyForce(pullDirection * pullForce);
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

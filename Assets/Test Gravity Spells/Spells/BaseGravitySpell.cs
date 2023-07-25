using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseGravitySpell : BaseProjectile
{
    [field: SerializeField] public float PullForce { get; protected set; }
    [field: SerializeField] public float PullRadius { get; protected set; }
    [SerializeField] protected bool falloff;

    [Header("Performance")]
    [SerializeField] protected float updatePullAblesDelay;

    private Rigidbody[] pullAbleObjects;

    protected override void OnLaunch()
    {
        base.OnLaunch();
        StartCoroutine(UpdatePullAbles());
        StartCoroutine(Pull());
    }

    public override void Die()
    {
        base.Die();
    }

    private IEnumerator UpdatePullAbles()
    {
        var wait = new WaitForSeconds(updatePullAblesDelay);

        while (isAlive)
        {
            var cols = Physics.OverlapSphere(transform.position, PullRadius, LayerMasks.PullAble);
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
                {

                    pullAbleObjects[i].AddForce(pullVector.normalized * PullForce / Mathf.Max(pullVector.magnitude/*sqrMagnitude*/, 1));
                }
                else
                    pullAbleObjects[i].AddForce(pullVector.normalized * PullForce);

            }

            yield return null;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, PullRadius);
    }
}

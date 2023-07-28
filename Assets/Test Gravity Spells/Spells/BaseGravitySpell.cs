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

    protected Rigidbody[] pullAbleObjects;

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
        while (isAlive)
        {
            var cols = Physics.OverlapSphere(transform.position, PullRadius, LayerMasks.PullAble);
            pullAbleObjects = new Rigidbody[cols.Length];

            for (int i = 0; i < cols.Length; i++)
                pullAbleObjects[i] = cols[i].GetComponent<Rigidbody>();

            yield return new WaitForSeconds(updatePullAblesDelay); ;
        }

    }

    private IEnumerator Pull()
    {
        while (isAlive)
        {
            foreach (var obj in pullAbleObjects)
            {
                if (obj != null)
                {
                    var pullVector = transform.position - obj.transform.position;

                    if (falloff)
                    {
                        obj.AddForce(pullVector.normalized * PullForce / Mathf.Max(pullVector.magnitude/*sqrMagnitude*/, 1));
                    }
                    else
                        obj.AddForce(pullVector.normalized * PullForce);
                }
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BasePushSpell : BaseProjectile
{
    [field: SerializeField] public float Force { get; protected set; }
    [field: SerializeField] public float Radius { get; protected set; }
    [SerializeField] protected bool falloff;


    protected void Push()
    {
        var cols = Physics.OverlapSphere(transform.position, Radius);

        for (int i = 0; i < cols.Length; i++)
        {
            var rb = cols[i].GetComponent<Rigidbody>();

            if (rb != null)
            {
                var pushVector = rb.transform.position - transform.position;

                if (falloff)
                    rb.AddForce(pushVector.normalized * Force / Mathf.Max(pushVector.sqrMagnitude, 1), ForceMode.Impulse);
                else
                    rb.AddForce(pushVector.normalized * Force, ForceMode.Impulse);
            }
        }
    }
    protected void Push(Vector3 direction)
    {
        var normalizedDirection = direction.normalized;

        var cols = Physics.OverlapSphere(transform.position, Radius);

        for (int i = 0; i < cols.Length; i++)
        {
            var rb = cols[i].GetComponent<Rigidbody>();

            if (rb != null)
            {
                if (falloff)
                {
                    var sqrDistance = (rb.transform.position - transform.position).sqrMagnitude;
                    rb.AddForce(normalizedDirection * Force / Mathf.Max(sqrDistance, 1f), ForceMode.Impulse);
                }
                else
                    rb.AddForce(normalizedDirection * Force, ForceMode.Impulse);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, Radius);
    }
}

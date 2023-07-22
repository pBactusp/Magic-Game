using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class PullAble : MonoBehaviour
{
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        gameObject.layer = LayerMask.NameToLayer("PullAble");
    }

    public void ActivateGravity(bool active)
    {
        rb.useGravity = active;
    }

    public void ApplyForce(Vector3 force, ForceMode forceMode = ForceMode.Force)
    {
        rb.AddForce(force, forceMode);
    }
}

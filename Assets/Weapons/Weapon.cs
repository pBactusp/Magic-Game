using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public abstract class Weapon : MonoBehaviour
{
    [Header("When attacking")]
    [SerializeField] protected Collider[] weaponColliders;
    public virtual float AttackDistance { get; protected set; }
    [field: SerializeField] public DamageData AttackDamage { get; private set; }

    [Header("When dropped")]
    [SerializeField] private float FloatToHandSpeed;
    [SerializeField] private Collider physicalCollider;
    private new Rigidbody rigidbody;


    public WeaponFamilies Family { get; protected set; }


    public Action OnReachedHand;
    public Action<HealthManager, DamageData> OnHit;

    protected void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    

    public void Drop()
    {
        transform.parent = null;
        rigidbody.useGravity = true;
        rigidbody.isKinematic = false;
        physicalCollider.enabled = true;
        gameObject.layer = LayerMask.NameToLayer("Interactable");
    }
    
    public void Equip(Transform parent)
    {
        StartCoroutine(FloatToHand(parent));
    }

    public IEnumerator FloatToHand(Transform parent)
    {
        rigidbody.useGravity = false;
        rigidbody.isKinematic = true;
        physicalCollider.enabled = false;
        gameObject.layer = LayerMask.NameToLayer("PlayerWeapon");


        bool arrivedAtHand = false;

        while (!arrivedAtHand)
        {
            transform.position = Vector3.Lerp(transform.position, parent.position, FloatToHandSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, parent.rotation, FloatToHandSpeed * Time.deltaTime);


            if (Vector3.Distance(transform.position, parent.position) < 0.1f)
                arrivedAtHand = true;

            yield return null;
        }

        transform.position = parent.position;
        transform.parent = parent;
        transform.localEulerAngles = Vector3.zero;

        OnReachedHand?.Invoke();
    }
}

public enum WeaponFamilies
{
    OneHandedMelee,
    OneHandedStaff
}

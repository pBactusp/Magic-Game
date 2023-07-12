using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : Weapon
{
    [field: SerializeField] public override float AttackDistance { get; protected set; }


    new private void Awake()
    {
        Family = WeaponFamilies.OneHandedMelee;
        base.Awake();
    }
    protected void Start()
    {
        SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        OnHit?.Invoke(other.gameObject.GetComponent<HealthManager>(), AttackDamage);
    }

    public void SetActive(bool active)
    {
        for (int i = 0; i < weaponColliders.Length; i++)
        {
            weaponColliders[i].enabled = active;
        }
    }

    new public void Drop()
    {
        SetActive(false);
        base.Drop();
    }
}

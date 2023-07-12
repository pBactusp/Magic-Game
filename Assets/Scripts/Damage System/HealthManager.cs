using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    public float PoiseRegenerationRate;
    [field: SerializeField] public float MaxHealth { get; private set; }
    [field: SerializeField] public float MaxPoise { get; private set; }
    [field: SerializeField] public Resistances Resistances { get; private set; }

    // For display only
    [field: SerializeField] public float Health { get; private set; }
    [field: SerializeField] public float Poise { get; private set; }

    //[Header("Optional")]
    //public HealthBar HealthBar;

    // Actions
    public Action OnDamaged;
    public Action OnDeath;
    public Action OnHealed;

    void Start()
    {
        Health = MaxHealth;
        Poise = MaxPoise;
    }

    public void RecieveDamage(DamageData damage)
    {
        Health -= GameManager.CalculateDamage(Resistances, damage);

        if (Health > 0)
        {
            OnDamaged?.Invoke();
            //HealthBar?.SetHealth(Health / MaxHealth);
            return;
        }

        Health = 0;
        //HealthBar?.SetHealth(Health / MaxHealth);
        OnDeath?.Invoke();
    }

    public void Heal(float amount)
    {
        Health += amount;

        if (Health > MaxHealth)
            Health = MaxHealth;

        //HealthBar?.SetHealth(Health / MaxHealth);
    }
}

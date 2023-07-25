using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;



    [Header("Components")]
    public Camera MainCamera;


    public Transform Player { get; private set; }




    void Awake()
    {
        if (Instance != null)
        {
            Debug.LogWarning("Game Manager already exists");
            Destroy(this);
            return;
        }

        Instance = this;

        // Find player object
        Player = GameObject.Find("Player").transform;
    }

    private void Start()
    {
        ApplyDefaultSettings();
    }
    void ApplyDefaultSettings()
    {
        // Template
    }


    

    public static float CalculateDamage(Resistances resistance, DamageData damage)
    {
        return damage.Physical * resistance.PhysicalModifier +
                damage.Fire * resistance.FireModifier +
                damage.Magic * resistance.MagicModifier;
    }

}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;


    [Header("Components")]
    public Camera MainCamera;

    [Header("For Transparent Objects")]
    [Range(0f, 1f)]
    public float MinTransparency;
    public float TransparencyChangeSpeed;

    [Header("For Selected Outline")]
    [Range(0f, 10f)]
    public float OutlineWidth;

    [Header("Read Only, Visible For Debugging")]
    public Transform SelectedObject;

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

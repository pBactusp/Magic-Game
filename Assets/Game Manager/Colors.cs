using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Colors : MonoBehaviour
{
    public static bool Exists = false;

    public static Color EnemyHover;
    public static Color EnemySelection;
    public static Color InteractableHover;
    public static Color InteractableSelection;

    public Color EnemyHoverColor;
    public Color EnemySelectionColor;
    public Color InteractableHoverColor;
    public Color InteractableSelectionColor;



    private void Awake()
    {
        if (Exists)
        {
            Destroy(this);
            return;
        }

        Exists = true;
        SetStaticColors();
    }


    private void OnValidate()
    {
        SetStaticColors();
    }

    private void SetStaticColors()
    {
        EnemyHover = EnemyHoverColor;
        EnemySelection = EnemySelectionColor;
        InteractableHover = InteractableHoverColor;
        InteractableSelection = InteractableSelectionColor;
    }
}

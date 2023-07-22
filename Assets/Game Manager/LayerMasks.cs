using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerMasks : MonoBehaviour
{
    public static bool Exists = false;

    public static LayerMask Player;
    public static LayerMask Obstruction;
    public static LayerMask Ground;
    public static LayerMask Enemy;
    public static LayerMask Interactable;
    public static LayerMask Selection;
    public static LayerMask Cutout;
    public static LayerMask PullAble;

    public LayerMask PlayerMask;
    public LayerMask ObstructionMask;
    public LayerMask GroundMask;
    public LayerMask EnemyMask;
    public LayerMask InteractableMask;
    public LayerMask SelectionMask;
    public LayerMask CutoutMask;
    public LayerMask PullAbleMask;

    private void Awake()
    {
        if (Exists)
        {
            Destroy(this);
            return;
        }

        Exists = true;
        SetStaticLayers();
    }

    private void OnValidate()
    {
        SetStaticLayers();
    }

    private void SetStaticLayers()
    {
        Player = PlayerMask;
        Obstruction = ObstructionMask;
        Ground = GroundMask;
        Enemy = EnemyMask;
        Interactable = InteractableMask;
        Selection = SelectionMask;
        Cutout = CutoutMask;
        PullAble = PullAbleMask;
    }


    public static bool MaskContainsLayer(int layer, LayerMask mask)
    {
        return mask.value == (mask.value | (1 << layer));
    }
}

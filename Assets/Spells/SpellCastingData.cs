using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GenericSpellData", menuName = "Spell Data/Generic Spell")]
public class SpellCastingData : ScriptableObject
{
    [field: SerializeField] public float CastTime { get; private set; }
    [field: SerializeField] public float MovementSpeedWhileCasting { get; private set; }
    [field: SerializeField] public float PlayerSlowdownTime { get; private set; } // The time it takes the player to get to "MovementSpeedWhileCasting" while casting this spell
    [field: SerializeField] public GameObject Spell { get; private set; }
    [field: SerializeField] public AnimatorOverrideController Animator { get; private set; }
    [field: SerializeField] public SpellOrigin Origin { get; private set; }
}

[CreateAssetMenu(fileName = "SelfSpellData", menuName = "Spell Data/Self Targeting Spell")]
public class SelfTargetingSpells : SpellCastingData
{

}


public enum SpellOrigin
{
    Null,
    RightHand,
    LeftHand,
    Chest
}
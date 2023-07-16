using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GenericSpellData", menuName = "Spell Data/Generic Spell")]
public class SpellCastingData : ScriptableObject
{
    [field: SerializeField] public float CastTime { get; private set; }
    [field: SerializeField] public float MovementSpeedWhileCasting { get; private set; }
    [field: SerializeField] public GameObject Spell { get; private set; }
}

[CreateAssetMenu(fileName = "SelfSpellData", menuName = "Spell Data/Self Targeting Spell")]
public class SelfTargetingSpells : SpellCastingData
{

}
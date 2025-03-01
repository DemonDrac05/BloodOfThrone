using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Item/Combat")]
public class CombatItem : BaseItem
{
    [Header("=== Combat Item Properties ==========")]
    public CombatItemType CombatItemType;
    public int currentLevel;

    [Header("=== Statistic Addition ==========")]
    public List<StatisticCategory> Statistics;

    public override int MaxStackable => 1;
}

[System.Serializable]
public class StatisticCategory
{
    public StatisticType Type;
    public ValueType ValueType;
    public float Value;
}

public enum CombatItemType
{
    None,
    Headwear,
    Jewelry,
    Armory,
    Amulet,
    Weapon
}

public enum RarityType
{
    None,
    Common,
    Uncommon,
    Epic,
    Rare,
    Legendary,
    Mythical
}

public enum StatisticType
{
    MaxHealth,
    HealthRecover,
    PhysicalDamage,
    PhysicalDefense,
    PhysicalPiercing,
    MagicalDamage,
    MagicalDefense,
    MagicalPiercing,
    CriticalChance,
    CriticalDamage,
    OmniVampChance,
    OmniVampValue
}

public enum ValueType
{
    Multiply,
    Additive
}
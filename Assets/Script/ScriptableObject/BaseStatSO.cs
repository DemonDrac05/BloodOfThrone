using UnityEngine;

public class BaseStatSO : ScriptableObject
{
    [Header("=== Basic Statistic ==========")]
    public float movementSpeed;
    public float maxHealth;

    [Header("=== Physics ==========")]
    public float physicalAtk;
    public float physicalDef;
    [Range(0f, 1f)] public float physicalPiercing;

    [Header("=== Magic ==========")]
    public float magicalAtk;
    public float magicalDef;
    [Range(0f, 1f)] public float magicalPiercing;

    [Header("=== Unique Statistic ==========")]
    [Range(0f, 1f)] public float criticalChance;
    [Range(0f, 9f)] public float criticalDamage;
}

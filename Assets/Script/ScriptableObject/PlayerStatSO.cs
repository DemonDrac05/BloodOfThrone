using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Statistic/Player")]
public class PlayerStatSO : BaseStatSO
{
    [Header("=== Addition Basic Stat ==========")]
    public float jumpForce;

    [Header("=== Addition Unique Stat ==========")]
    [Range(0f, 1f)] public float healthRec;
    [Range(0f, 1f)] public float omniVampChance;
    [Range(0f, 1f)] public float omniVampVal;
}

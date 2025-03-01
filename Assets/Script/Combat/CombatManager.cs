using UnityEngine;

public class CombatManager 
{
    public void ApplyPhysicDamage(BaseStatistic attacker, BaseStatistic defender) 
        => ApplyDamage(attacker, defender, AttackType.Physic);
    public void ApplyMagicDamage(BaseStatistic attacker, BaseStatistic defender) 
        => ApplyDamage(attacker, defender, AttackType.Magic);

    public void ApplyDamage(BaseStatistic attacker, BaseStatistic defender, AttackType type)
    {
        float typeOfAttackDamage = type switch
        {
            AttackType.Physic => attacker.PhysicalAtk,
            AttackType.Magic => attacker.MagicalAtk,
            _ => 0
        };

        float typeOfDefenseDamage = type switch
        {
            AttackType.Physic => defender.PhysicalDef,
            AttackType.Magic => defender.MagicalDef,
            _ => 0
        };

        float typeOfPiercingDamage = type switch
        {
            AttackType.Physic => attacker.PhysicalPiercing,
            AttackType.Magic => attacker.MagicalPiercing,
            _ => 0
        };

        float totalDamage = CalculatePiercingDamage(typeOfPiercingDamage, typeOfAttackDamage, typeOfDefenseDamage);
        totalDamage = CalculateCriticalDamage(attacker, totalDamage);
        defender.TakeHealthDamage(totalDamage);
    }

    private float CalculateCriticalDamage(BaseStatistic attacker, float baseDamage)
    {
        return Random.Range(0f, 1f) < attacker.CriticalChance ? baseDamage * attacker.CriticalDamage : baseDamage;
    }

    private float CalculatePiercingDamage(float attackerPiercing, float baseDamage, float defenderDefense)
    {
        return baseDamage - ((1 - attackerPiercing) * defenderDefense);
    }
}

public enum AttackType { Physic, Magic }

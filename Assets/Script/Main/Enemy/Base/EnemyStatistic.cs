using UnityEngine;

public class EnemyStatistic : BaseStatistic
{
    public void SetCriticalChance(float criticalChance) => CriticalChance = criticalChance;
    public void SetPhysicalPiercing(float physicalPiercing) => PhysicalPiercing = physicalPiercing;
}

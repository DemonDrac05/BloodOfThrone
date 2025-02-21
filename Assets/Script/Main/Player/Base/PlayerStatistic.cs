using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerStatistic : BaseStatistic
{
    private PlayerStatSO playerStatSO;

    public Dictionary<StatisticType, float> baseStatDicts;
    public Dictionary<StatisticType, float> playerStatDicts;

    public float JumpForce { get; private set; }
    public float HealthRec { get; private set; }
    public float OmniVampChance { get; private set; }
    public float OmniVampVal { get; private set; }
    public static PlayerStatistic Instance { get; private set; }

    public override async void Awake()
    {
        if (baseStatSO is PlayerStatSO)
        {
            playerStatSO = (PlayerStatSO)baseStatSO;
        }
        if (playerStatSO != null)
        {
            base.Awake();
            InitializeStatistic();
            InitializeBaseStatDict();
        }
        Instance = this;

        await UniTask.Yield();
    }

    private void InitializeStatistic()
    {
        JumpForce = playerStatSO.jumpForce;
        HealthRec = playerStatSO.healthRec;
        OmniVampChance = playerStatSO.omniVampChance;
        OmniVampVal = playerStatSO.omniVampVal;
    }

    private void InitializeBaseStatDict()
    {
        baseStatDicts = new Dictionary<StatisticType, float> 
        {
            { StatisticType.MaxHealth, playerStatSO.maxHealth },
            { StatisticType.HealthRecover, playerStatSO.healthRec },
            { StatisticType.PhysicalDamage, playerStatSO.physicalAtk },
            { StatisticType.PhysicalDefense, playerStatSO.physicalDef },
            { StatisticType.PhysicalPiercing, playerStatSO.physicalPiercing },
            { StatisticType.MagicalDamage, playerStatSO.magicalAtk },
            { StatisticType.MagicalDefense, playerStatSO.magicalDef },
            { StatisticType.MagicalPiercing, playerStatSO.magicalPiercing },
            { StatisticType.CriticalChance, playerStatSO.criticalChance },
            { StatisticType.CriticalDamage, playerStatSO.criticalDamage},
            { StatisticType.OmniVampChance, playerStatSO.omniVampChance},
            { StatisticType.OmniVampValue, playerStatSO.omniVampVal }
        };

        playerStatDicts = new Dictionary<StatisticType, float>
        {
            { StatisticType.MaxHealth, playerStatSO.maxHealth },
            { StatisticType.HealthRecover, playerStatSO.healthRec },
            { StatisticType.PhysicalDamage, playerStatSO.physicalAtk },
            { StatisticType.PhysicalDefense, playerStatSO.physicalDef },
            { StatisticType.PhysicalPiercing, playerStatSO.physicalPiercing },
            { StatisticType.MagicalDamage, playerStatSO.magicalAtk },
            { StatisticType.MagicalDefense, playerStatSO.magicalDef },
            { StatisticType.MagicalPiercing, playerStatSO.magicalPiercing },
            { StatisticType.CriticalChance, playerStatSO.criticalChance },
            { StatisticType.CriticalDamage, playerStatSO.criticalDamage},
            { StatisticType.OmniVampChance, playerStatSO.omniVampChance},
            { StatisticType.OmniVampValue, playerStatSO.omniVampVal }
        };
    }

    public bool CompleteDictImplement() => playerStatDicts.ContainsKey(StatisticType.OmniVampValue);

    private void RefreshPlayerStat()
    {
        Health = playerStatDicts[StatisticType.MaxHealth];
        HealthRec = playerStatDicts[StatisticType.HealthRecover];

        PhysicalAtk = playerStatDicts[StatisticType.PhysicalDamage];
        PhysicalDef = playerStatDicts[StatisticType.PhysicalDefense];
        PhysicalPiercing = playerStatDicts[StatisticType.PhysicalPiercing];

        MagicalAtk = playerStatDicts[StatisticType.MagicalDamage];
        MagicalDef = playerStatDicts[StatisticType.MagicalDefense];
        MagicalPiercing = playerStatDicts[StatisticType.MagicalPiercing];

        CriticalChance = playerStatDicts[StatisticType.CriticalChance];
        CriticalDamage = playerStatDicts[StatisticType.CriticalDamage];

        OmniVampChance = playerStatDicts[StatisticType.OmniVampChance];
        OmniVampVal = playerStatDicts[StatisticType.OmniVampValue];
    }

    public void AddStatValue(StatisticType type, ValueType valueType, float newVal)
    {
        if (playerStatDicts.TryGetValue(type, out var typeVal))
        {
            if (valueType == ValueType.Additive)
            {
                typeVal += newVal;
            }
            else if (valueType == ValueType.Multiply)
            {
                typeVal += baseStatDicts[type] * newVal;
            }
            playerStatDicts[type] = typeVal;
            RefreshPlayerStat();
        }
    }

    public void RemoveStatValue(StatisticType type, ValueType valueType, float newVal)
    {
        if (playerStatDicts.TryGetValue(type, out var typeVal))
        {
            if (valueType == ValueType.Additive)
            {
                typeVal -= newVal;
            }
            else if (valueType == ValueType.Multiply)
            {
                typeVal -= baseStatDicts[type] * newVal;
            }
            playerStatDicts[type] = typeVal;
            RefreshPlayerStat();
        }
    }

    public float GetSumStat(StatisticType type)
    {
        return playerStatDicts[type];
    }

    public float GetAddStat(StatisticType type)
    {
        return playerStatDicts[type] - baseStatDicts[type];
    }

    public bool HasMultiplyStatValue(StatisticType type)
    {
        return type == StatisticType.HealthRecover
            || type == StatisticType.PhysicalPiercing
            || type == StatisticType.MagicalPiercing
            || type == StatisticType.CriticalChance
            || type == StatisticType.CriticalDamage
            || type == StatisticType.OmniVampChance
            || type == StatisticType.OmniVampValue;
    }
}

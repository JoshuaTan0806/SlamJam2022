using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public enum StatType
{
    PercentValue,
    FlatValue,
    FinalMultiplier
}

[CreateAssetMenu(menuName = "Skill Tree/Stats")]
public class StatData : ScriptableObject
{
    public string InGameName;
    public string Description;
    public Sprite Icon;
    public Stat Stat;
    public float PercentValue;
    public float FlatValue;
    [Min(0)] public float FinalMultiplier = 1;

    [ReadOnly] public float TotalValue;

    public void ModifyStat(StatType statType, float value)
    {
        switch (statType)
        {
            case StatType.PercentValue:
                PercentValue += value;
                break;
            case StatType.FlatValue:
                FlatValue += value;
                break;
            case StatType.FinalMultiplier:
                FinalMultiplier *= value;
                break;
            default:
                break;
        }
    }
  
    public static StatData operator+ (StatData l, StatData r)
    {
        if (l.Stat != r.Stat)
            throw new System.Exception("Adding different stat");

        StatData statData = StatManager.NullStat(l.Stat);

        statData.PercentValue = l.PercentValue + r.PercentValue;
        statData.FlatValue = l.FlatValue + r.FlatValue;
        statData.FinalMultiplier = l.FinalMultiplier * r.FinalMultiplier;

        return statData;
    }

    public virtual void CalculateTotal()
    {
        float newValue = FlatValue;
        newValue *= (1 + (PercentValue / 100));
        newValue *= FinalMultiplier;

        TotalValue = newValue;
    }
}

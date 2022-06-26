using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StatManager", menuName = "StatManager", order = 1)]
public class StatManager : Factories.FactoryBase
{
    public static StatDictionary StatDictionary;
    [SerializeField] List<StatData> AllStats;

    public static StatData NullStat(Stat stat)
    {
        return Instantiate(StatDictionary[stat]); 
    }

    public static StatData CreateStat(Stat stat, StatType statType, float value)
    {
        StatData statData = Instantiate(StatDictionary[stat]);

        switch (statType)
        {
            case StatType.PercentValue:
                statData.ModifyStat(StatType.PercentValue, value);
                break;
            case StatType.FlatValue:
                statData.ModifyStat(StatType.FlatValue, value);
                break;
            case StatType.FinalMultiplier:
                statData.ModifyStat(StatType.FinalMultiplier, value);
                break;
            default:
                break;
        }

        return statData;
    }

    public override void Initialize()
    {
        StatDictionary = new StatDictionary();

        for (int i = 0; i < AllStats.Count; i++)
        {
            if (!StatDictionary.ContainsKey(AllStats[i].Stat))
                StatDictionary.Add(AllStats[i].Stat, Instantiate(AllStats[i]));
        }
    }
}
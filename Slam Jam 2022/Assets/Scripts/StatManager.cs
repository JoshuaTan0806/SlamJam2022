using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatManager : MonoBehaviour
{
    public static StatDictionary StatDictionary = new StatDictionary();
    [SerializeField] List<StatData> AllStats;

    private void Awake()
    {
        for (int i = 0; i < AllStats.Count; i++)
        {
            StatDictionary.Add(AllStats[i].Stat, Instantiate(AllStats[i]));
        }
    }

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
}
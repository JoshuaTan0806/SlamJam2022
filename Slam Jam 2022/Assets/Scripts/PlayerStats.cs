using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [System.Serializable]
    public class StatDictionary : SerializableDictionary<string, float> { }

    [SerializeField] StatDictionary stats = new StatDictionary();

    private void Awake()
    {
        if(GetStat(StatIDs.CURRENT_SATURATION) != 0)
            stats.Add(StatIDs.CURRENT_SATURATION, GetStat(StatIDs.MAX_SATURATION));
    }

    public void AddStat(string stat, float value)
    {
        if (!stats.ContainsKey(stat))
            stats.Add(stat, value);
        else
            stats[stat] += value;
    }

    public float GetStat(string stat)
    {
        return stats.ContainsKey(stat) ? stats[stat] : 0;
    }

    List<Summonable> currentActiveSummons = new List<Summonable>();
    public void AddSummon(Summonable summon)
    {

    }
}

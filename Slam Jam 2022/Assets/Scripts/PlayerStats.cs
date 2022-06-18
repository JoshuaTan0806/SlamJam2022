using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] protected StatDictionary stats = new StatDictionary();

    private void Awake()
    {
        if(GetStat(Stat.CurrentSaturation) != 0)
            stats.Add(Stat.CurrentSaturation, GetStat(Stat.MaxSaturation));
    }

    public void AddStat(Stat stat, float value)
    {
        if (!stats.ContainsKey(stat))
            stats.Add(stat, value);
        else
            stats[stat] += value;
    }

    public float GetStat(Stat stat)
    {
        return stats.ContainsKey(stat) ? stats[stat] : 0;
    }

    List<Summonable> currentActiveSummons = new List<Summonable>();
    public List<Summonable> CurrentActiveSummons => currentActiveSummons;
}

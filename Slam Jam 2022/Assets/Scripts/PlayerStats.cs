using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public Dictionary<Stat, float> stats = new Dictionary<Stat, float>();

    public void AddStat(Stat stat, float value)
    {
        if (!stats.ContainsKey(stat))
            stats.Add(stat, value);
        else
            stats[stat] += value;
    }
}

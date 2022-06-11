using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [System.Serializable]
    public class StatDictionary : SerializableDictionary<string, float> { }

    public StatDictionary stats = new StatDictionary();

    public void AddStat(string stat, float value)
    {
        if (!stats.ContainsKey(stat))
            stats.Add(stat, value);
        else
            stats[stat] += value;
    }
}

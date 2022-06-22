using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : PlayerStats
{
    public static Player instance;

    public int skillPoints
    {
        get
        {
            return _skillPoints;
        }
        set
        {
            _skillPoints = value;
            OnSkillPointsChanged?.Invoke();
        }
    }
    [SerializeField] int _skillPoints;
    public static System.Action OnSkillPointsChanged;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        Items.ItemInventory.onItemsRefresh += RefreshBonuses;
    }
    /// <summary>
    /// Rebuilds the players stats
    /// </summary>
    private void RefreshBonuses()
    {
        stats.Clear();
        //Combine stats
        AddStats(Items.ItemInventory.GetItemStats());

        RecalculateStats();
    }
    /// <summary>
    /// Calculates the players current stats.
    /// </summary>
    private void RecalculateStats()
    {
        //Read the stats from stats and re-calculate any stats
    }
    /// <summary>
    /// Adds a stat dictionary to our current stats
    /// </summary>
    /// <param name="stats"></param>
    private void AddStats(StatDictionary stats)
    {
        if (stats == null)
            return;
        //Add the stats to our current stats
        foreach (var stat in stats.Keys)
        {
            //if (stats.ContainsKey(stat))
            //    stats[stat] += stats[stat];
            //else
            //    stats[stat] = stats[stat];
        }
    }
}

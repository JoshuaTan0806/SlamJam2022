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

    public int NumberOfPots;

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

        foreach (var item in Items.ItemInventory.GetItemStats())
        {
            AddStat(item.Value);
        }

        RecalculateStats();
    }
    /// <summary>
    /// Calculates the players current stats.
    /// </summary>
    private void RecalculateStats()
    {
        //Read the stats from stats and re-calculate any stats
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            UsePotion();
        }
    }

    public void InitialisePotions()
    {
        NumberOfPots = Mathf.CeilToInt(GetStat(Stat.PotAmount).TotalValue);
    }

    void UsePotion()
    {
        if (NumberOfPots <= 0)
            return;

        NumberOfPots--;

        CurrentHealth += GetStat(Stat.Health).TotalValue;
    }
}

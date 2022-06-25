using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class Player : PlayerStats
{
    public static Player instance;
    [SerializeField] Image healthBar;
    [SerializeField] TextMeshProUGUI healthText;


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
        UpdateHealthBar();
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

    void UpdateHealthBar()
    {
        healthBar.fillAmount = CurrentHealth / GetStat(Stat.Health).TotalValue;
        healthText.text = CurrentHealth + "/" + GetStat(Stat.Health).TotalValue;
    }
}

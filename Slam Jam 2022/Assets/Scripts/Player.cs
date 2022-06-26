using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


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

    protected override void Start()
    {
        base.Start();
        RefreshBonuses();

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

        foreach (var s in SkillTreeManager.instance.nodes)
        {
            if (s.IsActive)
                s.ApplyPowerUps(true);
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            UsePotion();
        }

        for (int i = 0; i < SpillInputManager.SpillArray.Length; i++)
        {
            if (!SpillInputManager.SpillArray[i])
                break;
            if (!SpillInputManager.SpillArray[i].Spill)
                continue;

            SpillInputManager.SpillArray[i].SpillUpdate();
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

    protected override void Die()
    {
        base.Die();

        for (int i = 0; i < SpillInputManager.SpillArray.Length; i++)
            SpillInputManager.SpillArray[i].Spill = null;
    }
}

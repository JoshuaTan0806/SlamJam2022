using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [System.Serializable]
    public class BaseStatDict : SerializableDictionary<Stat, float> { }

    [SerializeField] protected StatDictionary stats = new StatDictionary();

    [Space]

    [SerializeField] protected BaseStatDict baseStats = new BaseStatDict();

    public event System.Action OnDamageTaken;
    [HideInInspector] public float CurrentHealth;
    public event System.Action OnDeath;
    public bool Dead { get; protected set; }

    private void Start()
    {
        foreach (var s in baseStats)
        {
            AddStat(StatManager.CreateStat(s.Key, StatType.FlatValue, s.Value));
        }
        CurrentHealth = 0;
    }

    public void AddStat(StatData statData)
    {
        if(!stats.ContainsKey(statData.Stat))
        {
            stats.Add(statData.Stat, statData);
        }
        else
        {
            stats[statData.Stat] = stats[statData.Stat] + statData;
        }

        stats[statData.Stat].CalculateTotal();
    }

    public StatData GetStat(Stat stat)
    {
        return stats.ContainsKey(stat) ? stats[stat] : StatManager.NullStat(stat);
    }

    List<Summonable> currentActiveSummons = new List<Summonable>();
    public List<Summonable> CurrentActiveSummons => currentActiveSummons;

    public List<PlayerStats> GetPlayerMinions()
    {
        List<PlayerStats> minions = new List<PlayerStats>();

        foreach (var m in CurrentActiveSummons)
        {
            PlayerStats s = m.GetComponent<PlayerStats>();

            if (s)
                minions.Add(s);
        }

        return minions;
    }


    /// <summary>
    /// Takes damage
    /// </summary>
    /// <param name="damage">How much damage we take</param>
    /// <param name="internalDamage">If the damage was internal, won't call damage taken event</param>
    public void TakeDamage(float damage, bool internalDamage = false)
    {
        if (Dead)
            return;

        CurrentHealth += damage;

        if (!internalDamage)
            OnDamageTaken?.Invoke();

        if (CurrentHealth > GetStat(Stat.Health).TotalValue)
            Die();
    }  

    void Die()
    {
        Dead = true;
        OnDeath?.Invoke();
    }
}

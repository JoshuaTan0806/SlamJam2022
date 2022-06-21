using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] protected StatDictionary stats = new StatDictionary();

    public event System.Action OnDamageTaken;
    [HideInInspector] public float CurrentHealth;

    private void Awake()
    {
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
        CurrentHealth -= damage;

        if (!internalDamage)
            OnDamageTaken?.Invoke();

        if (CurrentHealth < GetStat(Stat.Health).TotalValue)
            Die();
    }  

    void Die()
    {

    }
}

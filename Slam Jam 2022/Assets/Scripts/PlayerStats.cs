using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] protected StatDictionary stats = new StatDictionary();

    public event System.Action OnDamageTaken;

    private void Awake()
    {
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
        AddStat(Stat.CurrentHealth, damage);

        if (!internalDamage)
            OnDamageTaken?.Invoke();

        if (GetStat(Stat.CurrentHealth) > GetStat(Stat.TotalHealth))
            Die();
    }

    void Die()
    {

    }
}

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

	protected virtual void Start()
	{
		foreach (var s in baseStats)
		{
			AddStat(StatManager.CreateStat(s.Key, StatType.FlatValue, s.Value));
		}

		AddStat(StatManager.CreateStat(Stat.Spd, StatType.FinalMultiplier, 3));

		foreach (var s in StatManager.StatDictionary)
		{
			if (!stats.ContainsKey(s.Key))
			{
				AddStat(StatManager.CreateStat(s.Key, StatType.FlatValue, 1));
			}
		}

		CurrentHealth = 0;

		StartCoroutine(Regen());
	}

	public void AddStat(StatData statData)
	{
		if (!stats.ContainsKey(statData.Stat))
		{
			stats.Add(statData.Stat, statData);
		}
		else
		{
			stats[statData.Stat] = stats[statData.Stat] + statData;
		}

		stats[statData.Stat].CalculateTotal();
	}

	public void RemoveStat(StatData statData)
	{
		if (!stats.ContainsKey(statData.Stat))
		{
			stats.Add(statData.Stat, StatManager.NullStat(statData.Stat));
			stats[statData.Stat] = stats[statData.Stat] - statData;
		}
		else
		{
			stats[statData.Stat] = stats[statData.Stat] - statData;
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

	IEnumerator Regen()
	{
		while (!Dead)
		{
			yield return new WaitForSeconds(1);
			if (CurrentHealth - stats[Stat.Regen].TotalValue > 0)
			{
				CurrentHealth -= stats[Stat.Regen].TotalValue;

				Collider[] collider = Physics.OverlapSphere(transform.position, 3);

				for (int i = 0; i < collider.Length; i++)
				{
					if (collider[i].TryGetComponent(out PlayerStats stats))
					{
						if (stats != this)
							stats.TakeDamage(GetStat(Stat.DmgReflect).TotalValue);
					}
				}
			}
			else
				CurrentHealth = 0;
		}
	}

	/// <summary>
	/// Takes damage
	/// </summary>
	/// <param name="damage">How much damage we take</param>
	/// <param name="internalDamage">If the damage was internal, won't call damage taken event</param>
	public void TakeDamage(float damage, bool internalDamage = false, GameObject objectHit = null)
	{
		if (Dead)
			return;

		float totalDamage;

		if (stats[Stat.DmgReduc].TotalValue == 0)
			totalDamage = damage;
		else
			totalDamage = damage * 1 - 1 / stats[Stat.DmgReduc].TotalValue;

		if (objectHit != null)
		{
			if (objectHit.TryGetComponent(out Summonable summonable))
			{
				summonable.Summoner.CurrentHealth -= summonable.Summoner.GetStat(Stat.Lifesteal).TotalValue * totalDamage;
			}
		}

		CurrentHealth += totalDamage;

		//if (!internalDamage)
		OnDamageTaken?.Invoke();

		if (CurrentHealth > GetStat(Stat.Health).TotalValue)
			Die();
	}

	protected virtual void Die()
	{
		Dead = true;

		this.PerformAfterDelay(() => Destroy(gameObject), 3);

		OnDeath?.Invoke();
	}
}

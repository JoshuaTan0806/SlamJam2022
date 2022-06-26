using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Summonable), typeof(Rigidbody))]
public class Melee : MonoBehaviour
{
	Summonable summonable;
	Rigidbody rb;

	[Tooltip("How much damage is done to whoever is hit")]
	[SerializeField] float damage;

	List<PlayerStats> immuneEnemies = new List<PlayerStats>();

	[SerializeField] bool useKnockBack;
	[ShowIf("useKnockBack")]
	[SerializeField] float knockBack;
	[ShowIf("useKnockBack")]
	[SerializeField] float knockbackTime = 1;

	private void Start()
	{
		summonable = GetComponent<Summonable>();
		rb = GetComponent<Rigidbody>();
	}

	private void OnTriggerEnter(Collider other)
	{
		var stats = other.GetComponent<PlayerStats>();

		//If we didn't hit something with stats
		if (!stats)
			return;

		//If we hit ourselves
		if (stats == summonable.Summoner)
			return;

		//If we hit someone who has already been hit recently
		if (immuneEnemies.Contains(stats))
			return;

		//If we hit an ally summonable
		var s = other.GetComponent<Summonable>();
		if (s)
		{
			if (summonable.Summoner.CurrentActiveSummons.Contains(s))
				return;
		}

		//If our summoner was a summon
		var summonerAsSummonable = summonable.Summoner.GetComponent<Summonable>();
		if (summonerAsSummonable)
		{
			//If we hit their summoner
			if (stats == summonerAsSummonable.Summoner)
				return;

			//If we hit their summoners other summons
			if (s != null && summonerAsSummonable.Summoner)
				if (summonerAsSummonable.Summoner.CurrentActiveSummons.Contains(s))
					return;
		}

		HitEnemy(stats);
	}

	void HitEnemy(PlayerStats stats)
	{
		stats.TakeDamage(damage + summonable.Summoner.GetStat(Stat.ProjDmg).TotalValue);

		immuneEnemies.Add(stats);

		if (useKnockBack)
		{
			var dirToEnemy = (stats.transform.position - transform.position).normalized;
			dirToEnemy.y = 0;

			var aiManager = stats.GetComponent<AIManager>();
			if (aiManager)
			{
				aiManager.AddKnockback(dirToEnemy * knockBack, knockbackTime);
				return;
			}

			var player = stats.GetComponent<PlayerMovement>();
			if (player)
			{
				player.AddVelocity(dirToEnemy * knockBack, knockbackTime);
			}
		}
	}
}
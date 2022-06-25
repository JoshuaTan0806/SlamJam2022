using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Summonable), typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
	[SerializeField] float projectileSpeed;

	[Tooltip("How long of a delay before gravity is enabled for this projectile, (-1 for never on, 0 for instantly on, etc)")]
	[SerializeField] float gravityEnableDelay = -1;

	[Space]

	[Tooltip("How much damage is done to whoever is hit")]
	[SerializeField] float damage;

	[Space]

	[Tooltip("If true, projectile won't destroy when hitting an enemy")]
	[SerializeField] bool canPierce;
	[ShowIf("canPierce")]
	[Tooltip("How many enemies the projectile can pierce before the projectile gets destroyed, (-1 for infinite)")]
	[SerializeField] int pierceAmount = -1;

	[Space]
	[SerializeField] Summonable onDestroySummonable;

	Summonable summonable;
	Rigidbody rb;

	List<PlayerStats> immuneEnemies = new List<PlayerStats>();
	int enemiesHit;

	private void Start()
	{
		summonable = GetComponent<Summonable>();
		rb = GetComponent<Rigidbody>();

		rb.AddForce(transform.forward * projectileSpeed);

		rb.useGravity = false;

		if (gravityEnableDelay == 0)
		{
			rb.useGravity = true;
		}
		else if (gravityEnableDelay > 0)
		{
			this.PerformAfterDelay(() => rb.useGravity = true, gravityEnableDelay);
		}
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
		this.PerformAfterDelay(() => immuneEnemies.Remove(stats), 0.5f);

		enemiesHit++;

		if (canPierce)
		{
			if (pierceAmount < 0)
				return;

			if (enemiesHit < pierceAmount)
				return;
		}

		DestroyProjectile();
	}

	void DestroyProjectile()
	{
		if (onDestroySummonable != null)
		{
			var inst = Instantiate(onDestroySummonable);
			inst.Initialise(summonable.Summoner, summonable.Spill);
		}

		summonable.DestroySummon();
	}
}

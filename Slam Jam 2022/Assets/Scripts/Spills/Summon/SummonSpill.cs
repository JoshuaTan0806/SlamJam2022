using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SummonSpill", menuName = "Spills/SummonSpill")]
public class SummonSpill : GenericSpill
{
	[Header("Summon")]
	[SerializeField] Summonable summon;


	[Tooltip("The Lifetime of the summon, leave as -1 for infinite")]
	[SerializeField] float summonLifeTime = -1;

	[Tooltip("The max amount of summons that can be active at once, leave as -1 for infinite")]
	[SerializeField] int maxSummons = -1;
	int currentSummons = 0;

	[Tooltip("How many get summoned at once")]
	[SerializeField] int numToSummon = 1;

	[Space]

	[SerializeField] bool parentToPlayer;

	public override bool CanCastSpell(PlayerStats caster)
	{
		if (base.CanCastSpell(caster))
			return false;

		if (maxSummons > 0 && currentSummons >= maxSummons)
			return false;

		return true;
	}

	public override bool Cast(PlayerStats caster)
	{
		if (!base.Cast(caster))
			return false;



		return true;
	}
}
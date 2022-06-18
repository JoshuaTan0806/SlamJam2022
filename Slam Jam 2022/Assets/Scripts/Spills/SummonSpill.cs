using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SummonSpill", menuName = "Spills/SummonSpill")]
public class SummonSpill : GenericSpill
{
	[Header("Summon")]
	[SerializeField] GameObject summon;

	[Tooltip("The Lifetime of the summon, leave as -1 for infinite")]
	[SerializeField] float summonLifeTime = -1;

	[Tooltip("The max amount of summons that can be active at once, leave as -1 for infinite")]
	[SerializeField] int maxSummons = -1;

	public override bool Cast(PlayerStats caster)
	{
		return base.Cast(caster);
	}
}
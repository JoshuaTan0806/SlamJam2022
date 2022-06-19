using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SummonSpill", menuName = "Spills/SummonSpill")]
public class SummonSpill : GenericSpill
{
	[Header("Summon")]
	[SerializeField] Summonable summon;


	[Tooltip("The max amount of summons that can be active at once, leave as -1 for infinite")]
	[SerializeField] int maxSummons = -1;
	int currentSummons = 0;

	[Tooltip("How many get summoned at once")]
	[SerializeField] int numToSummon = 1;

	public override bool CanCastSpell(PlayerStats caster)
	{
		if (maxSummons > 0 && currentSummons >= maxSummons)
			return false;

		if (!base.CanCastSpell(caster))
			return false;

		return true;
	}

	public override bool Cast(PlayerStats caster)
	{
		if (!base.Cast(caster))
			return false;

		var inst = Instantiate(summon);
		inst.Initialise(caster, this);
		caster.CurrentActiveSummons.Add(inst);
		currentSummons++;

		inst.OnSummonDestroyed += () => RemoveSummon(inst);

		return true;
	}

	void RemoveSummon(Summonable s)
	{
		//s.OnSummonDestroyed -= () => RemoveSummon(s);

		currentSummons--;
	}
}
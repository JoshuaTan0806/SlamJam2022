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

		if(summon.TryGetComponent(out Projectile projectile))
        {
			numToSummon += Mathf.CeilToInt(caster.GetStat(Stat.AddProj).TotalValue);
        }

        for (int i = 0; i < numToSummon; i++)
        {
			GameManager.instance.StartCoroutine(Cast(caster, numToSummon));
		}
	
		return true;
	}

	IEnumerator Cast(PlayerStats caster, int num)
	{
		yield return new WaitForSeconds(0.2f * num);
		var inst = Instantiate(summon);
		inst.Initialise(caster, this);
		currentSummons++;

		inst.OnSummonDestroyed += () => RemoveSummon(inst);
	}

	void RemoveSummon(Summonable s)
	{
		//s.OnSummonDestroyed -= () => RemoveSummon(s);

		currentSummons--;
	}
}
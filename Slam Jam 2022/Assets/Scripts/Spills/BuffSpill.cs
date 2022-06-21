using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Buff Spill", menuName = "Spills/Buff Spill")]
public class BuffSpill : GenericSpill
{
	[System.Serializable]
	struct Buff
	{
		public Stat stat;
		public StatType statType;
		public float val;
	}

	[ShowIf("spellType", SpellType.SingleCast), Min(1)]
	[SerializeField] float buffLifeTime = 1;
	float spellActiveLifeTime;

	[SerializeField] Buff[] buffs;

	bool casted = false;
	PlayerStats caster;

	public override bool CanCastSpell(PlayerStats caster)
	{
		if (!base.CanCastSpell(caster))
			return false;

		return casted == false;
	}

	public override bool Cast(PlayerStats caster)
	{
		if (!base.Cast(caster))
			return false;

		this.caster = caster;

		if (spellType == SpellType.Toggle)
		{
			if (spellToggled)
				AddStats();
			else
				FinishSpell();
		}
		else
		{
			casted = true;

			AddStats();
		}

		return true;
	}

	void AddStats()
	{
		foreach (var b in buffs)
		{
			caster.AddStat(StatManager.CreateStat(b.stat, b.statType, b.val));
		}
	}
	void FinishSpell()
	{
		foreach (var b in buffs)
        {
			caster.AddStat(StatManager.CreateStat(b.stat, b.statType, -b.val));
		}

		casted = false;
	}

	public override void UpdateSpell()
	{
		base.UpdateSpell();

		if(spellType == SpellType.SingleCast)
		{
			spellActiveLifeTime += Time.deltaTime;

			if(spellActiveLifeTime > buffLifeTime)
			{
				FinishSpell();
			}
		}

	}
}

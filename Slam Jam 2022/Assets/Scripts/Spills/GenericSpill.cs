using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericSpill : ScriptableObject
{
    [System.Serializable]
    public enum SpellType
    {
        SingleCast,
        Toggle,
    }

    [Tooltip("How much health casting uses")]
    [SerializeField] protected float castCost;

    [Tooltip("How long the cast takes to go on cooldown")]
    [SerializeField] protected float castCooldown;
    float castTimer = -1;
    public float CastCooldown => castCooldown;
    public float CastTimer => castTimer;

    [SerializeField] protected SpellType spellType;
    protected bool spellToggled;
    public bool SpellToggled => spellToggled;

    public virtual bool CanCastSpell(PlayerStats caster)
    {
        if(caster.CurrentHealth + castCost * caster.GetStat(Stat.SpellCostMult).TotalValue > caster.GetStat(Stat.Health).TotalValue)
            return false;

        if (CastTimer > 0)
            return false;

        return true;
    }

    public virtual bool Cast(PlayerStats caster)
    {
        if (!CanCastSpell(caster))
            return false;

        caster.CurrentHealth -= castCost * caster.GetStat(Stat.SpellCostMult).TotalValue;

        spellToggled = !spellToggled;

        return true;
    }

    public virtual void UpdateSpell()
    {
        castTimer -= Time.deltaTime;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericSpill : ScriptableObject
{
    public enum SpellType
    {
        SingleCast,
        Toggle,
    }

    [Tooltip("How much health casting uses")]
    [SerializeField] protected float castCost;

    [Tooltip("How long it takes to cast the spell")]
    [SerializeField] protected float castTime = 0;

    [Tooltip("How long the cast takes to go on cooldown")]
    [SerializeField] protected float castCooldown;

    [SerializeField] protected SpellType spellType;

    public virtual bool Cast(PlayerStats caster)
    {
        if (caster.GetStat(StatIDs.CURRENT_SATURATION) + castCost > caster.GetStat(StatIDs.MAX_SATURATION))
        {
            return false;
        }

        caster.AddStat(StatIDs.CURRENT_SATURATION, castCost);

        return true;
    }
}

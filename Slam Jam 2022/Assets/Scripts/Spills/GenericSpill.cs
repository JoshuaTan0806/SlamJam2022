using Sirenix.OdinInspector;
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

    public Sprite icon;

    [Tooltip("How much health casting uses")]
    [SerializeField] protected float castCost;

    [Tooltip("How long the cast takes to go on cooldown")]
    [SerializeField] protected float castCooldown;
    float castTimer = -1;
    public float CastCooldown => castCooldown;
    public float CastTimer => castTimer;

    [SerializeField] protected SpellType spellType;

    [ShowIf("spellType", SpellType.SingleCast)]
    [SerializeField] protected float spellDuration;

    protected bool spellToggled;
    public bool SpellToggled => spellToggled;

    [Header("FX")]
    [SerializeField] protected GameObject particles;
    protected GameObject particleInstance;

    public virtual bool CanCastSpell(PlayerStats caster)
    {
        if (caster == null)
            return false;

        if (caster is Player)
            if (caster.CurrentHealth + castCost * CastMultiplier(caster) > caster.GetStat(Stat.Health).TotalValue)
                return false;

        if (CastTimer > 0)
            return false;

        return true;
    }

    float CastMultiplier(PlayerStats caster)
    {
        if (caster.GetStat(Stat.SpellCostMult).TotalValue == 0)
            return 1;
        else
            return 1 - 1 / caster.GetStat(Stat.SpellCostMult).TotalValue;
    }

    float CastSpeed(PlayerStats caster)
    {
        if (caster.GetStat(Stat.CastSpd).TotalValue == 0)
            return 0;
        else
            return 1 / caster.GetStat(Stat.CastSpd).TotalValue;
    }

    public virtual bool Cast(PlayerStats caster)
    {
        if (!CanCastSpell(caster))
            return false;

        castTimer = CastSpeed(caster);

        if (caster is Player)
            caster.CurrentHealth += castCost * CastMultiplier(caster);

        spellToggled = !spellToggled;

        if (particles)
        {
            if (spellType == SpellType.SingleCast)
            {
                particleInstance = Instantiate(particles, caster.transform);

                var ps = particleInstance.GetComponentInChildren<ParticleSystem>();
                if (ps)
                {
                    ps.Play();
                }

                if (spellDuration > 0)
                {
                    caster.PerformAfterDelay(() =>
                    {
                        if (ps)
                        {
                            ps.enableEmission = false;
                        }

                        Destroy(particleInstance, 2);

                    }, spellDuration);
                }
                else
                {
                    Destroy(particleInstance, 5);
                }
            }
            else
            {
                if (SpellToggled)
                {
                    particleInstance = Instantiate(particles, caster.transform);

                    var ps = particleInstance.GetComponentInChildren<ParticleSystem>();
                    if(ps)
                        ps.Play();
                }
                else
                {
                    var ps = particleInstance.GetComponentInChildren<ParticleSystem>();

                    if (ps)
                    {
                        ps.enableEmission = false;
                    }
                    Destroy(particleInstance, 2);
                }
            }
        }

        return true;
    }

    public virtual void UpdateSpell()
    {
        castTimer -= Time.deltaTime;
    }
}

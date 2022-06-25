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

        if (particles)
        {
            if (spellType == SpellType.SingleCast)
            {
                particleInstance = Instantiate(particles, caster.transform);

                var ps = particleInstance.GetComponentInChildren<ParticleSystem>();
                ps.Play();

                if (spellDuration > 0)
                {
                    caster.PerformAfterDelay(() =>
                    {
                        ps.enableEmission = false;

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
                    ps.Play();
                }
                else
                {
                    var ps = particleInstance.GetComponentInChildren<ParticleSystem>();

                    ps.enableEmission = false;
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

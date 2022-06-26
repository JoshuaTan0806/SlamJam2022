using System.Collections.Generic;

/// <summary>
/// Contains constants specific to Stats
/// </summary>
public static class StatIDs
{
    public static string StatToName(Stat stat)
    {
        switch (stat)
        {
            default:
                return stat.ToString();
        }
    }
}
/// <summary>
/// Dictionary for storing stats.
/// </summary>
[System.Serializable]
public class StatDictionary : SerializableDictionary<Stat, StatData> { }

public enum Stat
{
    Health,
    ProjDmg,
    AddProj,
    CastSpd,
    Lifesteal,
    Spd,
    AOEDmg,
    AOEArea,
    KnockbackReduc,
    DmgReduc,
    PotAmount,
    PotMult,
    SpellCostMult,
    DmgReflect,
    Regen,
    Knockback,
    ProjSpd
}
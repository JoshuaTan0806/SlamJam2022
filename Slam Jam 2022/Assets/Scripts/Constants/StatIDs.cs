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

    public static Dictionary<Stat, string> StatToInGameName = new Dictionary<Stat, string>()
    {
        {Stat.TotalHealth, "Total Permeability" },
        {Stat.CurrentHealth, "Current Permeability"},
        {Stat.FlatHealth, "Flat Permeability"},
        {Stat.PercentHealth, "Percent Permeability"},
        {Stat.TotalDamage, "Total Pressure"},
        {Stat.FlatDamage, "Flat Pressure"},
        {Stat.PercentDamage, "Percent Pressure"},
        {Stat.TotalSpeed, "Total Current"},
        {Stat.FlatSpeed, "Flat Current"},
        {Stat.PercentSpeed, "Percent Current"},
        {Stat.TotalSplashDamage, "Total Splash Damage"},
        {Stat.PercentSplashDamage, "Percent Splash Damage"},
        {Stat.TotalSplashArea, "Total Splash Area"},
        {Stat.PercentSplashArea, "Percent Splash Area"},
        {Stat.TotalCriticalStrikeChance, "Total Cascade Chance"},
        {Stat.FlatCriticalStrikeChance, "Flat Cascade Chance"},
        {Stat.PercentCriticalStrikeChance, "Percent Cascade Chance"},
        {Stat.TotalCriticalStrikeMultiplier, "Total Cascade Multiplier"},
        {Stat.FlatCriticalStrikeMultiplier, "Flat Cascade Multiplier"},
        {Stat.PercentCriticalStrikeMultiplier, "Percent Cascade Multiplier"},
        {Stat.TotalRegeneration, "Total Density"},
        {Stat.FlatRegeneration, "Flat Density"},
        {Stat.PercentRegeneration, "Percent Density"},
        {Stat.TotalProjectileSpeed, "Total Torrent Speed"},
        {Stat.FlatProjectileSpeed, "Flat Torrent Speed"},
        {Stat.PercentProjectileSpeed, "Percent Torrent Damage"},
        {Stat.TotalProjectileDamage, "Total Damage"},
        {Stat.FlatProjectileDamage, "Flat Torrent Damage"},
        {Stat.PercentProjectileDamage, "Percent Torrent Damage"},
        {Stat.TotalDamageReduction, "Total Insolubility"},
        {Stat.FlatDamageReduction, "Flat Insolubility"},
        {Stat.PercentDamageReduction, "Percent Insolubility"}
    };
}
/// <summary>
/// Dictionary for storing stats.
/// </summary>
[System.Serializable]
public class StatDictionary : SerializableDictionary<Stat, float> { }

public enum Stat
{
    TotalHealth,
    CurrentHealth, 
    FlatHealth, //FlatPermeability
    PercentHealth, //PercentPermeability,
    TotalDamage,
    FlatDamage, // FlatPressure
    PercentDamage, //PercentPressure
    TotalSpeed,
    FlatSpeed, // FlatCurrent
    PercentSpeed,
    TotalSplashDamage,
    PercentSplashDamage,  //PercentSplashPressure
    TotalSplashArea,
    PercentSplashArea, //aoe size
    TotalCriticalStrikeChance,
    FlatCriticalStrikeChance, //FlatCascadeChance, 
    PercentCriticalStrikeChance, //PercentCascadeChance, 
    TotalCriticalStrikeMultiplier,
    FlatCriticalStrikeMultiplier, //FlatCascadeMultiplier
    PercentCriticalStrikeMultiplier, //PercentCascadeMultiplier, 
    TotalRegeneration,
    FlatRegeneration, //Density
    PercentRegeneration,
    TotalProjectileSpeed,
    FlatProjectileSpeed,
    PercentProjectileSpeed,
    TotalProjectileDamage,
    FlatProjectileDamage, //torrent
    PercentProjectileDamage,
    TotalDamageReduction, //TotalInsolubility
    FlatDamageReduction,
    PercentDamageReduction
}


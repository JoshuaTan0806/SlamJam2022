
/// <summary>
/// Contains constants specific to Stats
/// </summary>
public static class StatIDs
{
    public static string StatToName(Stat stat)
    {
        switch(stat)
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
public class StatDictionary : SerializableDictionary<Stat, float> { }

public enum Stat
{
    TotalHealth,
    CurrentHealth, 
    FlatHealth, //FlatPermeability, //health
    PercentHealth, //PercentPermeability,
    TotalDamage,
    Damage, // FlatPressure, //damage
    PercentDamage, //PercentPressure,
    TotalSpeed,
    FlatSpeed, // FlatCurrent, //speed
    PercentSpeed,
    TotalSplashDamage,
    PercentSplashDamage,  //PercentSplashPressure, //aoe damage
    TotalSplashArea,
    PercentSplashArea, //aoe size
    TotalCriticalStrikeChance,
    FlatCriticalStrikeChance, //FlatCascadeChance, //crit chance
    PercentCriticalStrikeChance, //PercentCascadeChance, 
    TotalCriticalStrikeMultiplier,
    FlatCriticalStrikeMultiplier, //FlatCascadeMultiplier, //crit damage
    PercentCriticalStrikeMultiplier, //PercentCascadeMultiplier, 
    TotalRegeneration,
    FlatRegeneration, //FlatEvapourationSpeed, //regen
    PercentRegeneration,
    TotalProjectileSpeed,
    FlatProjectileSpeed,
    PercentProjectileSpeed,
    TotalProjectileDamage,
    FlatProjectileDamage, //torrent
    PercentProjectileDamage
}
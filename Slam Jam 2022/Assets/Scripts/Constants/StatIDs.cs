
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
    CurrentSaturation = 'H',
    MaxSaturation = 'P',
    Damage = 'D',
    PercentDamage = 'd',
    Speed = 'S',
    PercentSpeed = 's'
}
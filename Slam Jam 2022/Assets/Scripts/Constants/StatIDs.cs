
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

public enum Stat
{
    CurrentSaturation = 'H',
    MaxSaturation = 'P',
}
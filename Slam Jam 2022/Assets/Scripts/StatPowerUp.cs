using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Skill Tree/Stat power up")]
public class StatPowerUp : PowerUp
{
    [SerializeField] Stat stat;
    [SerializeField] float increase;

    private void OnValidate()
    {
        Name = increase + " " + stat.name;
    }

    public override void ApplyPowerUp()
    {
        Player.instance.AddStat(stat, increase);
    }

    public override void UnapplyPowerUp()
    {
        Player.instance.AddStat(stat, -increase);
    }
}

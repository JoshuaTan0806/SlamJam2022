using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(menuName = "Skill Tree/Stat power up")]
public class StatPowerUp : PowerUp
{
    [SerializeField] Stat stat;
    [SerializeField] float increase;

    public override void ApplyPowerUp()
    {
        Player.instance.AddStat(stat, increase);
    }

    public override void UnapplyPowerUp()
    {
        Player.instance.AddStat(stat, -increase);
    }

    [Button]
    public void AutoGenerateName()
    {
        string statStr = stat.ToString();

        if(statStr.Contains("Percent"))
        {
            statStr = statStr.Replace("Percent", "% ");
            Name = increase + statStr;
        }
        else
        {
            Name = increase + " " + statStr;

        }
    }
}

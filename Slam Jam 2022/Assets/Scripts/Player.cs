using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerStats))]
public class Player : MonoBehaviour
{
    public static Player instance;

    PlayerStats stats;
    [SerializeField] int skillPoints;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        stats = GetComponent<PlayerStats>();
    }

    public void AddStat(Stat stat, float value)
    {
        stats.AddStat(stat, value);
    }

    public void AddSkillPoint()
    {
        skillPoints++;
    }

    public bool HasSkillPoint()
    {
        return skillPoints > 0;
    }

    public void RemoveSkillPoint()
    {
        skillPoints--;
    }
}

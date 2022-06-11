using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerStats))]
public class Player : MonoBehaviour
{
    public static Player instance;

    PlayerStats stats;
    public int skillPoints;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        stats = GetComponent<PlayerStats>();
    }

    public void AddStat(string stat, float value)
    {
        stats.AddStat(stat, value);
    }
}

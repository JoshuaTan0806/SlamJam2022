using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerStats))]
public class Player : MonoBehaviour
{
    public static Player instance;
    PlayerStats stats;
    

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
}

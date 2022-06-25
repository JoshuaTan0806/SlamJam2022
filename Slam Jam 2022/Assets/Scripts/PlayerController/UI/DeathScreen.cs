using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathScreen : MonoBehaviour
{
    private void Awake()
    {
        Player.instance.OnDeath += Die;
    }

    void Die()
    {
        Player.instance.OnDeath -= Die;


    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathScreen : MonoBehaviour
{
    [SerializeField] GameObject deathUI;

    LevelLoader loader;

    private void Start()
    {
        loader = GetComponentInParent<LevelLoader>();

        deathUI.SetActive(false);

        Player.instance.OnDeath += Die;
    }

    void Die()
    {
        Player.instance.OnDeath -= Die;

        this.PerformAfterDelay(() => 
        {
            deathUI.SetActive(true);

            this.PerformAfterDelay(() => 
            {
                loader.LoadLevel(0);
            }, 4);
        },1);
    }
}

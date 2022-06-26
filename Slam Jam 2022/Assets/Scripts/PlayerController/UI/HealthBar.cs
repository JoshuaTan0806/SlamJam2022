using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] Image healthBar;
    [SerializeField] TextMeshProUGUI healthText;

    // Start is called before the first frame update
    void Start()
    {
        Player.instance.OnDamageTaken += UpdateHealthBar;

        this.PerformAtEndOfFrame(UpdateHealthBar);
    }

    void UpdateHealthBar()
    {
        healthBar.fillAmount = Player.instance.CurrentHealth / Player.instance.GetStat(Stat.Health).TotalValue;
        healthText.text = Player.instance.CurrentHealth + "/" + Player.instance.GetStat(Stat.Health).TotalValue;
    }
}

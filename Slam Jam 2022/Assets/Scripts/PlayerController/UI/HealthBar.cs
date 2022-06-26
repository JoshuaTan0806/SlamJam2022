using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
	[SerializeField] Image healthBar;
	[SerializeField] TextMeshProUGUI healthText;

	[Space]
	[SerializeField] bool isAI;

	PlayerStats stats;
	// Start is called before the first frame update
	void Start()
	{
		if (isAI)
		{
			stats = GetComponentInParent<PlayerStats>();

			stats.OnDeath += () => Destroy(gameObject);
		}
		else
		{
			stats = Player.instance;
		}

		stats.OnDamageTaken += UpdateHealthBar;

		this.PerformAtEndOfFrame(UpdateHealthBar);
	}

	void UpdateHealthBar()
	{
		healthBar.fillAmount = stats.CurrentHealth / stats.GetStat(Stat.Health).TotalValue;
		healthText.text = Mathf.Floor(stats.CurrentHealth) + "/" + stats.GetStat(Stat.Health).TotalValue;
	}
}

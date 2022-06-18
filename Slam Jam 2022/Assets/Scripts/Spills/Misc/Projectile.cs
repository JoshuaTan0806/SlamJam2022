using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Summonable), typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
	[SerializeField] float projectileSpeed;

	[Tooltip("How long of a delay before gravity is enabled for this projectile, (-1 for never on, 0 for instantly on, etc)")]
	[SerializeField] float gravityEnableDelay = -1;

	Summonable summonable;

	Rigidbody rb;

	private void Awake()
	{
		summonable = GetComponent<Summonable>();
		rb = GetComponent<Rigidbody>();

		rb.AddForce(transform.forward * projectileSpeed);

		rb.useGravity = false;

		if (gravityEnableDelay == 0)
		{
			rb.useGravity = true;
		}
		else if (gravityEnableDelay > 0)
		{
			this.PerformAfterDelay(() => rb.useGravity = true, gravityEnableDelay);
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		var stats = other.GetComponent<PlayerStats>();

		if (!stats)
			return;

		if (stats == summonable.Summoner)
			return;


	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AIEyes), typeof(PlayerStats))]
public class AIManager : MonoBehaviour
{
	const float sightCheckCooldown = 0.2f;
	class IdleActionDict : SerializableDictionary<AIIdleState, int> { }
	class TriggeredActionDict : SerializableDictionary<AITriggeredState, int> { }

	[Tooltip("All the idle actions which the AI can do, with weightings")]
	[SerializeField] IdleActionDict idleActions;

	[SerializeField] TriggeredActionDict triggeredActions;

	AIState currentState;
	AIEyes eyes;
	PlayerStats stats;

	bool triggered = false;

	public bool IsAlly = false;

	private void Awake()
	{
		eyes.GetComponent<AIEyes>();
		stats.GetComponent<PlayerStats>();

		PickIdleState();
	}

	private void Update()
	{
		if (currentState)
		{
			currentState.UpdateState();

			if (currentState.StateDuration > 0 && currentState.StateTimer >= currentState.StateDuration)
			{
				if (triggered)
				{
					PickTriggeredState();
				}
				else
				{
					PickIdleState();
				}
			}
		}
	}

	bool CanSeeEnemies(out PlayerStats enemy)
	{
		var playerStats = Player.instance.GetComponent<PlayerStats>();

		if (!IsAlly)
		{
			//Check the player and all of his summons

			if (eyes.CanSeeTransform(Player.instance.transform))
			{
				enemy = playerStats;

				return true;
			}

			foreach (var minion in playerStats.GetPlayerMinions())
			{
				if (eyes.CanSeeTransform(minion.transform))
				{
					enemy = minion;

					return true;
				}
			}
		}
		else
		{
			//Check for the first thing which isn't an enemy

			var sight = eyes.GetAllInSight();
			foreach (var e in sight)
			{
				var stats = e.GetComponent<PlayerStats>();

				if (!stats)
					continue;

				if (stats == playerStats)
					continue;

				if (playerStats.GetPlayerMinions().Contains(stats))
					continue;

				enemy = stats;

				return true;
			}
		}

		enemy = null;

		return false;
	}

	#region State Switching
	void PickIdleState()
	{
		int max = 0;
		foreach (var i in idleActions)
		{
			max += i.Value;
		}

		int rand = Random.Range(0, max);
		foreach (var i in idleActions)
		{
			rand -= i.Value;
			if (rand <= 0)
			{
				PickState(i.Key);
				return;
			}
		}
	}

	void PickTriggeredState()
	{
		int max = 0;
		foreach (var i in idleActions)
		{
			max += i.Value;
		}

		int rand = Random.Range(0, max);
		foreach (var i in idleActions)
		{
			rand -= i.Value;
			if (rand <= 0)
			{
				PickState(i.Key);
				return;
			}
		}
	}

	void PickState(AIState state)
	{
		if (currentState)
			currentState.StopState();

		currentState = state;

		currentState.TriggerState();
	}
	#endregion

	//Activate idleAction
	//When duration over, repeat

	//When spot player
	//Stop idleAction
	//Pick triggeredAction

	//If lose sight of player
	//Return to idleAction
}
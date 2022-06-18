using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AIEyes), typeof(PlayerStats))]
public class AIManager : MonoBehaviour
{
	const float sightCheckCooldown = 0.2f;
	[System.Serializable]
	class IdleActionDict : SerializableDictionary<AIIdleState, int> { }
	[System.Serializable]
	class TriggeredActionDict : SerializableDictionary<AITriggeredState, int> { }

	[Tooltip("All the idle actions which the AI can do, with weightings")]
	[SerializeField] IdleActionDict idleActions = new IdleActionDict();

	[SerializeField] TriggeredActionDict triggeredActions = new TriggeredActionDict();

	AIState currentState;
	AIEyes aiEyes;

	PlayerStats aiTarget;
	public PlayerStats AiTarget => aiTarget;

	bool triggered = false;

	public bool IsAlly = false;

	private float sightTimer;

	private void Awake()
	{
		aiEyes = GetComponent<AIEyes>();

		PickIdleState();
	}

	private void Update()
	{
		sightTimer -= Time.deltaTime;
		if(sightTimer <= 0)
		{
			sightTimer = sightCheckCooldown;

			if (triggered)
			{
				//If we can't spot an enemy while triggered
				if(!CanSeeEnemies(out aiTarget))
				{
					Debug.Log("Back to idle");

					triggered = false;
					PickIdleState();
				}
			}
			else
			{
				//If we can spot an enemy while idle
				if(CanSeeEnemies(out aiTarget))
				{
					Debug.Log("Back to triggered");

					triggered = true;
					PickTriggeredState();
				}
			}
		}

		if (currentState)
		{
			currentState.UpdateState(aiTarget);

			if (currentState.StateDuration > 0 && currentState.StateTimer >= currentState.StateDuration)
			{
				Debug.Log("State finished");

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
		var playerStats = Player.instance;

		if (!IsAlly)
		{
			//Check the player and all of his summons
			if (aiEyes.CanSeeTransform(Player.instance.transform))
			{
				enemy = playerStats;

				return true;
			}

			foreach (var minion in playerStats.GetPlayerMinions())
			{
				if (aiEyes.CanSeeTransform(minion.transform))
				{
					enemy = minion;

					return true;
				}
			}
		}
		else
		{
			//Check for the first thing which isn't an enemy
			var sight = aiEyes.GetAllInSight();
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
		int max = 1;
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
		transform.LookAt(new Vector3(aiTarget.transform.position.x, transform.position.y, aiTarget.transform.position.z));

		int max = 1;
		foreach (var i in triggeredActions)
		{
			max += i.Value;
		}

		int rand = Random.Range(0, max);
		foreach (var i in triggeredActions)
		{
			rand -= i.Value;
			if (rand <= 0)
			{
				PickState(i.Key);
				break;
			}
		}
	}

	void PickState(AIState state)
	{
		if (currentState)
			currentState.StopState();

		currentState = Instantiate(state);

		currentState.TriggerState(this);

		Debug.Log($"State changed to: {currentState.name}");
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
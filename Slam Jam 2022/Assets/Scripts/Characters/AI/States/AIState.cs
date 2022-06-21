using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIState : ScriptableObject
{
	[Tooltip("Duration the AI will be in the state (-1 for until otherwise stated)")]
	[SerializeField] protected float stateDuration = -1;
	public float StateDuration => stateDuration;

	[Space]

	[Tooltip("If true, action will remove itself after single use")]
	[SerializeField] protected bool singleTime = false;
	public bool SingleTime => singleTime;

	float stateTimer;
	public float StateTimer => stateTimer;

	NavMeshAgent agent;
	protected NavMeshAgent Agent => agent;

	public virtual void TriggerState(AIManager ai)
	{
		agent = ai.GetComponent<NavMeshAgent>();
	}

	public virtual void StopState()
	{

	}

	public virtual void UpdateState(PlayerStats target)
	{
		stateTimer += Time.deltaTime;
	}

	protected void FinishState()
	{
		stateDuration = 1;
		stateTimer = 2;
	}
}

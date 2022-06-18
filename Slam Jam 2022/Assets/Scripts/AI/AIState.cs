using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIState : ScriptableObject
{
	[Tooltip("Duration the AI will be in the state (-1 for until otherwise stated)")]
	[SerializeField] float stateDuration = -1;
	public float StateDuration => stateDuration;

	float stateTimer;
	public float StateTimer => stateTimer;

	public virtual void TriggerState()
	{

	}

	public virtual void StopState()
	{

	}

	public virtual void UpdateState()
	{
		stateTimer += Time.deltaTime;
	}
}

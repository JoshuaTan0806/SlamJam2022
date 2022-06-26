using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "Wander", menuName = "AI/State/Wander")]
public class AIWander : AIIdleState
{
	[SerializeField] float wanderRange;

	Vector3 currentTargetPos;

	public override void TriggerState(AIManager ai)
	{
		this.ai = ai;

		base.TriggerState(ai);

		GetRandomSpot();
	}

	void GetRandomSpot()
	{
		for (int i = 0; i < 10; i++)
		{
			var randSpot = (Random.insideUnitCircle * wanderRange);
			currentTargetPos = Agent.transform.position + new Vector3(randSpot.x, 0, randSpot.y);

			if (NavMesh.SamplePosition(currentTargetPos, out var hit, wanderRange * 2, NavMesh.AllAreas))
			{
				ai.SetMoving(true);
				Agent.SetDestination(currentTargetPos);
				return;
			}
		}

		FinishState();
	}

	public override void UpdateState(PlayerStats target)
	{
		base.UpdateState(target);

		//if(Agent.pathStatus == NavMeshPathStatus.PathComplete)
		//if (Vector3.Distance(Agent.transform.position, currentTargetPos) < 0.2f)
		if(Agent.remainingDistance <= 0.1f)
			FinishState();
	}
}

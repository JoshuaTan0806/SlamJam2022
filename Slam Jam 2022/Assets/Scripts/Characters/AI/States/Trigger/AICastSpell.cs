using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CastSpell", menuName = "AI/State/Cast Spell")]
public class AICastSpell : AITriggeredState
{
    [Header("Casting")]
    [SerializeField] GenericSpill toCast;
    GenericSpill castInst;

    [Tooltip("After casting the spell, how long is the AI stuck before returning to normal")]
    [SerializeField] float afterCastCooldown;

    [Header("Movement")]
    [SerializeField] bool castOnSpot = true;

    [HideIf("castOnSpot")]
    [Tooltip("How close to the target we wish to be in order to cast")]
    [SerializeField] float idealCastDistance;

    [HideIf("castOnSpot")]
    [Tooltip("If true, will snap to the target when casting")]
    [SerializeField] bool snapOnCast = false;

    bool casted = false;
    Vector3 currentDestination;

    AIManager ai;

    public override void TriggerState(AIManager ai)
    {
        this.ai = ai;
        base.TriggerState(ai);

        castInst = Instantiate(toCast);

        if (castOnSpot)
        {
            Cast();
        }
        else
        {
            Agent.stoppingDistance = idealCastDistance - 1f;
            SetDestination();
        }
    }

    public override void UpdateState(PlayerStats target)
    {
        base.UpdateState(target);

        if (casted)
            return;

        if (Vector3.Distance(currentDestination, target.transform.position) > 1.5f)
        {
            SetDestination();
        }

        if (Vector3.Distance(target.transform.position, ai.transform.position) < idealCastDistance)
        {
            Cast();
        }
    }

    protected void Cast()
    {
        if (casted)
            return;

        var stats = Agent.GetComponent<PlayerStats>();

        castInst.Cast(stats);

        casted = true;

        if (afterCastCooldown > 0)
            stats.PerformAfterDelay(FinishState, afterCastCooldown);
        else
            FinishState();
    }

    void SetDestination()
    {
        currentDestination = ai.AiTarget.transform.position;
        Agent.SetDestination(currentDestination);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIIdleState : AIState
{
    public void Enter(AIZombieAgent agent)
    {

    }

    public void Exit(AIZombieAgent agent)
    {
    }

    public AIStateID GetStateID()
    {
        return AIStateID.Idle;
    }

    public void Update(AIZombieAgent agent)
    {
        Vector3 targetDirection = agent.targetTransform.position - agent.transform.position;

        if (targetDirection.magnitude > agent.config.maxSightDistance)
        {
            return;
        }

        Vector3 agentDirection = agent.transform.forward;
        targetDirection.Normalize();
        float dotProduct = Vector3.Dot(targetDirection, agentDirection);

        if (dotProduct > 0.0f)
        {
            agent.stateMachine.ChangeState(AIStateID.ChasePlayer);
        }
    }
}

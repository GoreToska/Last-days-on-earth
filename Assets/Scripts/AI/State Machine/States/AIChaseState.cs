using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIChaseState : AIState
{
    public void Enter(BaseAIAgent agent)
    {
        agent.navMeshAgent.isStopped = false;
        agent.animator.CrossFade("Blend Tree", 0.1f);
        Debug.Log("EnterChase");
    }

    public void Exit(BaseAIAgent agent)
    {
    }

    public AIStateID GetStateID()
    {
        return AIStateID.ChasePlayer;
    }

    public void Update(BaseAIAgent agent)
    {
        if (agent.targetSystem.HasTarget &&
            Vector3.Distance(agent.transform.position, agent.targetSystem.TargetPosition) <= agent.navMeshAgent.stoppingDistance)
        {
            agent.stateMachine.ChangeState(AIStateID.Attack);
        }
        else
        {
            if (!agent.targetSystem.HasTarget && !agent.navMeshAgent.pathPending)
            {
                agent.stateMachine.ChangeState(AIStateID.Idle);
                return;
            }
        }

        if (agent.targetSystem.HasTarget)
        {
            agent.navMeshAgent.destination = agent.targetSystem.TargetPosition;
        }
    }
}

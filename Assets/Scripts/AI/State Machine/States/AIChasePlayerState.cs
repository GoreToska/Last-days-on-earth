using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIChasePlayerState : AIState
{
    public void Enter(AIZombieAgent agent)
    {
        agent.navMeshAgent.isStopped = false;
        agent.animator.CrossFade("Blend Tree", 0.1f);
    }

    public void Exit(AIZombieAgent agent)
    {
    }

    public AIStateID GetStateID()
    {
        return AIStateID.ChasePlayer;
    }

    public void Update(AIZombieAgent agent)
    {
        if (Vector3.Distance(agent.transform.position, agent.targetTransform.position) <= agent.navMeshAgent.stoppingDistance)
        {
            agent.stateMachine.ChangeState(AIStateID.Attack);
        }
        else
        {
            if(agent.sensor.Objects.Count < 1 && !agent.navMeshAgent.pathPending)
            {
                agent.stateMachine.ChangeState(AIStateID.Idle);
                return;
            }

            agent.targetTransform = agent.sensor.Objects[0].transform;
            agent.navMeshAgent.destination = agent.targetTransform.position;
        }
    }
}

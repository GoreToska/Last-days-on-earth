using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIChasePlayerState : AIState
{
    public void Enter(AIZombieAgent agent)
    {
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
        agent.navMeshAgent.destination = agent.targetTransform.position;
    }
}

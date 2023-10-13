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
        if (agent.sensor.Objects.Count < 1)
        {
            return;
        }

        agent.stateMachine.ChangeState(AIStateID.ChasePlayer);
    }
}

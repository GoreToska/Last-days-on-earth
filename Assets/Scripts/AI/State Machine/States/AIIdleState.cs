using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIIdleState : AIState
{
    private float timer = 0f;
    public void Enter(BaseAIAgent agent)
    {
    }

    public void Exit(BaseAIAgent agent)
    {
        timer = 0f;
    }

    public AIStateID GetStateID()
    {
        return AIStateID.Idle;
    }

    public void Update(BaseAIAgent agent)
    {
        if (agent.sensor.Objects.Count > 0)
        {
            agent.stateMachine.ChangeState(AIStateID.ChasePlayer);
            return;
        }

        timer += Time.deltaTime;

        if(timer > agent.timeToStartRoaming) 
        {
            agent.stateMachine.ChangeState(AIStateID.Roaming);
        }
    }
}

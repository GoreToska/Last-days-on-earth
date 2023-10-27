using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIIdleState : AIState
{
    private float timer = 0f;
    public void Enter(AIZombieAgent agent)
    {
        Debug.Log("Enter Idle State");
    }

    public void Exit(AIZombieAgent agent)
    {
        timer = 0f;
    }

    public AIStateID GetStateID()
    {
        return AIStateID.Idle;
    }

    public void Update(AIZombieAgent agent)
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
            Debug.Log("Roam");
        }
    }
}

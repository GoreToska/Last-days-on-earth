using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIRoamingState : AIState
{
    public void Enter(AIZombieAgent agent)
    {
        agent.navMeshAgent.destination = RandomNavmeshLocation(agent.roamingRadius, agent);
    }

    public void Exit(AIZombieAgent agent)
    {
    }

    public AIStateID GetStateID()
    {
        return AIStateID.Roaming;
    }

    public void Update(AIZombieAgent agent)
    {
        if (agent.sensor.Objects.Count > 0)
        {
            agent.stateMachine.ChangeState(AIStateID.ChasePlayer);
        }

        if (agent.navMeshAgent.pathStatus == NavMeshPathStatus.PathComplete)
        {
            agent.stateMachine.ChangeState(AIStateID.Idle);
        }
    }

    public Vector3 RandomNavmeshLocation(float radius, AIZombieAgent agent)
    {
        Vector3 randomDirection = Random.insideUnitSphere * radius;
        randomDirection += agent.transform.position;
        NavMeshHit hit;
        Vector3 finalPosition = Vector3.zero;
        
        if (NavMesh.SamplePosition(randomDirection, out hit, radius, 1))
        {
            finalPosition = hit.position;
        }

        return finalPosition;
    }
}

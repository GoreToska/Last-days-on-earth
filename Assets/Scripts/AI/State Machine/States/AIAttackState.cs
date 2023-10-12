using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAttackState : AIState
{
    public void Enter(AIZombieAgent agent)
    {
    }

    public void Exit(AIZombieAgent agent)
    {
        agent.isAttacking = false;
    }

    public AIStateID GetStateID()
    {
        return AIStateID.Attack;
    }

    public void Update(AIZombieAgent agent)
    {
        Debug.Log("Attacking update");

        //  light attack
        if (!agent.isAttacking)
        {
            agent.isAttacking = true;
            agent.navMeshAgent.isStopped = true;
            agent.animator.CrossFade("Zombie_Light_Attack_01", 0.1f);
        }


        if (Vector3.Distance(agent.transform.position, agent.targetTransform.position) > agent.navMeshAgent.stoppingDistance)
        {
            Debug.Log("Change to Chase");
            agent.stateMachine.ChangeState(AIStateID.ChasePlayer);
        }
    }
}

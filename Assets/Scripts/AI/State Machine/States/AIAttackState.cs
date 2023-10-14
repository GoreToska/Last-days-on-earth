using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AIAttackState : AIState
{
    public void Enter(AIZombieAgent agent)
    {
        Attack(agent);
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
        if(agent.isAttacking)
        {
            return;
        }

        if (agent.targetSystem.HasTarget && agent.targetSystem.Target.tag == "Player" && agent.targetSystem.Target.GetComponent<PlayerStatusManager>().isDead)
        {
            agent.stateMachine.ChangeState(AIStateID.Idle);
            return;
        }

        if (agent.targetSystem.HasTarget && Vector3.Distance(agent.transform.position, agent.targetSystem.TargetPosition) > agent.navMeshAgent.stoppingDistance)
        {
            Debug.Log("Change to Chase");
            agent.stateMachine.ChangeState(AIStateID.ChasePlayer);
        }
        else
        {
            //  light attack
            if (!agent.isAttacking)
            {
                Attack(agent);
                return;
            }
        }
    }
    
    private void Attack(AIZombieAgent agent)
    {
        agent.isAttacking = true;
        agent.navMeshAgent.isStopped = true;
        agent.animator.CrossFade("Zombie_Light_Attack_01", 0.1f);
    }
}

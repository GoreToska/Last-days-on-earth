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
        agent.animator.CrossFade("Blend Tree", 0.1f);
        Debug.Log("Exit Attack state");
    }

    public AIStateID GetStateID()
    {
        return AIStateID.Attack;
    }

    public void Update(AIZombieAgent agent)
    {
        if (agent.isAttacking)
        {
            return;
        }

        if (!agent.targetSystem.HasTarget)
        {
            Debug.Log("Exit Attack state to Idle state");
            agent.stateMachine.ChangeState(AIStateID.Idle);
            return;
        }

        if (!agent.targetSystem.Target.TryGetComponent<CharacterStatusManager>(out var status) || status.IsDead)
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAttackState : AIState
{
    public void Enter(BaseAIAgent agent)
    {
        //LightAttack(agent);
    }

    public void Exit(BaseAIAgent agent)
    {
        agent.isAttacking = false;
        agent.animator.CrossFade("Blend Tree", 0.1f);
    }

    public AIStateID GetStateID()
    {
        return AIStateID.Attack;
    }

    public void Update(BaseAIAgent agent)
    {
        if (agent.isAttacking)
        {
            return;
        }

        if (!agent.targetSystem.HasTarget)
        {
            agent.stateMachine.ChangeState(AIStateID.Idle);

            return;
        }

        if (!agent.targetSystem.Target.TryGetComponent<IDamagable>(out var status) || status.IsDead)
        {
            agent.targetSystem.ForgetTarget(agent.targetSystem.Target);
            agent.stateMachine.ChangeState(AIStateID.Idle);

            return;
        }

        if (agent.targetSystem.HasTarget && Vector3.Distance(agent.transform.position, agent.targetSystem.TargetPosition) > agent.navMeshAgent.stoppingDistance)
        {
            agent.stateMachine.ChangeState(AIStateID.ChasePlayer);
        }
        else
        {
            PerformAttack(agent);
        }
    }

    private void PerformAttack(BaseAIAgent agent)
    {
        if (!agent.isAttacking)
        {
            var a = Random.Range(0, 101);
            Debug.Log(a);

            if (a >= 50)
            {
                LightAttack(agent);
                return;
            }
            else
            {
                HeavyAttack(agent);
                return;
            }
        }
    }

    private void LightAttack(BaseAIAgent agent)
    {
        agent.transform.LookAt(agent.targetSystem.TargetPosition, Vector3.up);
        var meleeAgent = agent as MeleeAIAgent;
        meleeAgent.AIAttack.PerformLightMeleeAttack(agent, agent.config.lightDamage);
    }

    private void HeavyAttack(BaseAIAgent agent)
    {
        agent.transform.LookAt(agent.targetSystem.TargetPosition, Vector3.up);
        var meleeAgent = agent as MeleeAIAgent;
        meleeAgent.AIAttack.PerformHeavyMeleeAttack(agent, agent.config.heavyDamage);
    }
}

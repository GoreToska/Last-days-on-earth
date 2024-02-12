using UnityEngine;

public class AIMeleeAttackState : IAIState
{
    public void Enter(BaseAIAgent agent)
    {
        //LightAttack(agent);
    }

    public void Exit(BaseAIAgent agent)
    {
        agent.IsAttacking = false;
        agent.Animator.CrossFade("Blend Tree", 0.1f);
    }

    public AIStateID GetStateID()
    {
        return AIStateID.MeleeAttack;
    }

    public void Update(BaseAIAgent agent)
    {
        if (agent.IsAttacking)
        {
            return;
        }

        if (!agent.TargetSystem.HasTarget)
        {
            agent.StateMachine.ChangeState(AIStateID.Idle);

            return;
        }

        if (!agent.TargetSystem.Target.TryGetComponent<IDamagable>(out var status) || status.IsDead)
        {
            agent.TargetSystem.ForgetTarget(agent.TargetSystem.Target);
            agent.StateMachine.ChangeState(AIStateID.Idle);

            return;
        }

        if (agent.TargetSystem.HasTarget && Vector3.Distance(agent.transform.position, agent.TargetSystem.TargetPosition) > agent.Config.AttackDistance)
        {
            agent.StateMachine.ChangeState(AIStateID.ChasePlayer);
        }
        else
        {
            PerformAttack(agent);
        }
    }

    private void PerformAttack(BaseAIAgent agent)
    {
        if (!agent.IsAttacking)
        {
            var a = Random.Range(0, 101);

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
        agent.transform.LookAt(agent.TargetSystem.TargetPosition, Vector3.up);
        var meleeAgent = agent as MeleeAIAgent;
        meleeAgent.AIAttack.PerformLightMeleeAttack(agent, agent.Config.LightDamage);
    }

    private void HeavyAttack(BaseAIAgent agent)
    {
        agent.transform.LookAt(agent.TargetSystem.TargetPosition, Vector3.up);
        var meleeAgent = agent as MeleeAIAgent;
        meleeAgent.AIAttack.PerformHeavyMeleeAttack(agent, agent.Config.HeavyDamage);
    }
}

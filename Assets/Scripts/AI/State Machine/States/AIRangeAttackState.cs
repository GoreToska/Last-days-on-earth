using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIRangeAttackState : IAIState
{
	private RangeAiAgent rangeAgent;

	public void Enter(BaseAIAgent agent)
	{
		if(rangeAgent == null)
			rangeAgent = agent as RangeAiAgent;

		agent.IsAttacking = false;
		agent.Animator.CrossFade("Blend Tree", 0.1f);
		agent.NavMeshAgent.isStopped = true;
	}

	public void Exit(BaseAIAgent agent)
	{
	}

	public AIStateID GetStateID()
	{
		return AIStateID.RangeAttack;
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
			agent.StateMachine.ChangeState(AIStateID.RangeIdle);

			return;
		}

		if (agent.TargetSystem.HasTarget && Vector3.Distance(agent.transform.position, agent.TargetSystem.TargetPosition) > agent.Config.AttackDistance)
		{
			agent.StateMachine.ChangeState(AIStateID.RangeChasePlayer);
		}
		else
		{
			PerformAttack(agent);
			agent.StateMachine.ChangeState(AIStateID.RangeChasePlayer);
		}
	}

	private void PerformAttack(BaseAIAgent agent)
	{
		// shot logic 
		var rangeAgent = agent as RangeAiAgent;
		
		agent.transform.LookAt(agent.TargetSystem.TargetPosition, Vector3.up);
		rangeAgent.RangeWeapon.Attack(agent.TargetSystem.Target);
	}
}

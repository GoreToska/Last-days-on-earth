using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Mathematics;
using UnityEngine;

public class AIRangeAttackState : IAIState
{
	private RangeAiAgent _rangeAgent;

	public void Enter(BaseAIAgent agent)
	{
		if (_rangeAgent == null)
			_rangeAgent = agent as RangeAiAgent;

		agent.IsAttacking = false;
		//agent.Animator.CrossFade("Blend Tree", 0.1f);
		agent.NavMeshAgent.isStopped = true;
	}

	public void Exit(BaseAIAgent agent)
	{
	}

	public AIStateID GetStateID()
	{
		return AIStateID.RangeAttack;
	}

	public async void Update(BaseAIAgent agent)
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

		if (_rangeAgent.RangeWeapon.Bullets == 0)
		{
			agent.StateMachine.ChangeState(AIStateID.Reload);
		}

		if (agent.TargetSystem.HasTarget && Vector3.Distance(agent.transform.position, agent.TargetSystem.TargetPosition) > agent.Config.AttackDistance)
		{
			agent.StateMachine.ChangeState(AIStateID.RangeChasePlayer);
		}
		else
		{
			await PerformAttack(agent);
		}
	}

	private async Task PerformAttack(BaseAIAgent agent)
	{
		// shot logic

		agent.IsAttacking = true;
		_rangeAgent.RangeWeapon.Attack(agent.TargetSystem.Target, agent.AIAnimation, _rangeAgent.Config.LightDamage);

		await Task.Delay(_rangeAgent.AttackInterval * 1000);

		if (agent.Status.IsDead)
			return;

		agent.IsAttacking = false;
		agent.StateMachine.ChangeState(AIStateID.RangeChasePlayer);
	}
}

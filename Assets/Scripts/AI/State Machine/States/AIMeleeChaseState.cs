using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMeleeChaseState : IAIState
{
	public void Enter(BaseAIAgent agent)
	{
		agent.NavMeshAgent.isStopped = false;
		agent.Animator.CrossFade("Blend Tree", 0.1f);

		if (agent.TargetSystem.HasTarget)
			agent.NavMeshAgent.destination = agent.TargetSystem.TargetPosition;
	}

	public void Exit(BaseAIAgent agent)
	{
	}

	public AIStateID GetStateID()
	{
		return AIStateID.ChasePlayer;
	}

	public void Update(BaseAIAgent agent)
	{
		if (agent.TargetSystem.HasTarget &&
			Vector3.Distance(agent.transform.position, agent.TargetSystem.TargetPosition) <= agent.Config.AttackDistance)
		{
			agent.StateMachine.ChangeState(AIStateID.MeleeAttack);
		}
		else
		{
			if (!agent.TargetSystem.HasTarget)
			{
				agent.StateMachine.ChangeState(AIStateID.Idle);
				return;
			}
		}

		if (agent.TargetSystem.HasTarget)
		{
			agent.NavMeshAgent.destination = agent.TargetSystem.TargetPosition;
		}
	}
}

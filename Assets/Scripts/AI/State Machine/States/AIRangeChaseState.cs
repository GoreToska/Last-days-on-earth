using UnityEngine;

public class AIRangeChaseState : IAIState
{
	public void Enter(BaseAIAgent agent)
	{
		agent.NavMeshAgent.isStopped = false;
		agent.Animator.CrossFade("Blend Tree", 0.1f);
	}

	public void Exit(BaseAIAgent agent)
	{
	}

	public AIStateID GetStateID()
	{
		return AIStateID.RangeChasePlayer;
	}

	public void Update(BaseAIAgent agent)
	{
		if (agent.TargetSystem.HasTarget &&
			Vector3.Distance(agent.transform.position, agent.TargetSystem.TargetPosition) <= agent.Config.AttackDistance
			&& agent.Sensor.IsInSight(agent.TargetSystem.Target))
		{
			agent.StateMachine.ChangeState(AIStateID.RangeAttack);
		}
		else
		{
			if (!agent.TargetSystem.HasTarget && !agent.NavMeshAgent.pathPending)
			{
				agent.StateMachine.ChangeState(AIStateID.RangeIdle);
				return;
			}
		}

		if (agent.TargetSystem.HasTarget)
		{
			agent.NavMeshAgent.destination = agent.TargetSystem.TargetPosition;
		}
	}
}

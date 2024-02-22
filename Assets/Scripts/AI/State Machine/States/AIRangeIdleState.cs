using UnityEngine;

public class AIRangeIdleState : IAIState
{
	private float timer = 0f;
	public void Enter(BaseAIAgent agent)
	{
		agent.NavMeshAgent.isStopped = true;
	}

	public void Exit(BaseAIAgent agent)
	{
		timer = 0f;
	}

	public AIStateID GetStateID()
	{
		return AIStateID.RangeIdle;
	}

	public void Update(BaseAIAgent agent)
	{
		if (agent.Sensor.Objects.Count > 0 || agent.TargetSystem.HasTarget)
		{
			agent.StateMachine.ChangeState(AIStateID.RangeChasePlayer);
			return;
		}

		if(agent.NavMeshAgent.isStopped)
		{
			timer += Time.deltaTime;
		}

		if (timer > agent.TimeToStartRoaming)
		{
			agent.StateMachine.ChangeState(AIStateID.RangeRoaming);
		}
	}
}

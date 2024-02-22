using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIRoamingState : IAIState
{
	public void Enter(BaseAIAgent agent)
	{
		agent.NavMeshAgent.destination = RandomNavmeshLocation(agent.RoamingRadius, agent);
	}

	public void Exit(BaseAIAgent agent)
	{
	}

	public AIStateID GetStateID()
	{
		return AIStateID.Roaming;
	}

	public void Update(BaseAIAgent agent)
	{
		if (agent.Sensor.Objects.Count > 0)
		{
			agent.StateMachine.ChangeState(AIStateID.ChasePlayer);
		}

		if (agent.NavMeshAgent.pathStatus == NavMeshPathStatus.PathComplete)
		{
			agent.StateMachine.ChangeState(AIStateID.Idle);
		}
	}

	public Vector3 RandomNavmeshLocation(float radius, BaseAIAgent agent)
	{
		Vector3 randomDirection = Random.insideUnitSphere * radius;
		randomDirection += agent.transform.position;
		NavMeshHit hit;
		Vector3 finalPosition = Vector3.zero;

		if (NavMesh.SamplePosition(randomDirection, out hit, radius, 1))
		{
			finalPosition = hit.position;
		}

		return finalPosition;
	}
}

public class AIRangeRoamingState : IAIState
{
	public void Enter(BaseAIAgent agent)
	{
		agent.NavMeshAgent.destination = RandomNavmeshLocation(agent.RoamingRadius, agent);
		agent.NavMeshAgent.isStopped = false;
	}

	public void Exit(BaseAIAgent agent)
	{
	}

	public AIStateID GetStateID()
	{
		return AIStateID.RangeRoaming;
	}

	public void Update(BaseAIAgent agent)
	{
		if (agent.TargetSystem.HasTarget)
		{
			agent.StateMachine.ChangeState(AIStateID.RangeChasePlayer);
		}

		agent.StateMachine.ChangeState(AIStateID.RangeIdle);
	}

	public Vector3 RandomNavmeshLocation(float radius, BaseAIAgent agent)
	{
		Vector3 randomDirection = Random.insideUnitSphere * radius;
		randomDirection += agent.transform.position;
		NavMeshHit hit;
		Vector3 finalPosition = Vector3.zero;

		if (NavMesh.SamplePosition(randomDirection, out hit, radius, 1))
		{
			finalPosition = hit.position;
		}

		return finalPosition;
	}
}

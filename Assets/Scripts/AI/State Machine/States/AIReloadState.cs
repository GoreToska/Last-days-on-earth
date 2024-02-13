using System.Threading.Tasks;

public class AIReloadState : IAIState
{
	private RangeAiAgent _rangeAgent;

	public async void Enter(BaseAIAgent agent)
	{
		if (_rangeAgent == null)
			_rangeAgent = agent as RangeAiAgent;

		agent.NavMeshAgent.isStopped = true;
		agent.IsAttacking = false;
		_rangeAgent.AIAnimation.PlayRifleReloadAnimation();

		await Wait(4);

		if (agent.Status.IsDead)
			return;

		_rangeAgent.RangeWeapon.Reload();
		agent.StateMachine.ChangeState(AIStateID.ChasePlayer);
	}

	public void Exit(BaseAIAgent agent)
	{

	}

	public AIStateID GetStateID()
	{
		return AIStateID.Reload;
	}

	public void Update(BaseAIAgent agent)
	{

	}

	private async Task Wait(int seconds)
	{
		await Task.Delay(seconds * 1000);
	}
}

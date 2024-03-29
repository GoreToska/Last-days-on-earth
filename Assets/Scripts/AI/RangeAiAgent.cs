using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeAiAgent : BaseAIAgent
{
	public IAIRangeWeapon RangeWeapon;
	public int AttackInterval = 3;

	public override void Awake()
	{
		base.Awake();

		RangeWeapon = GetComponentInChildren<IAIRangeWeapon>();
	}

	public override void Start()
	{
		base.Start();

		StateMachine.RegisterState(new AIRangeIdleState());
		StateMachine.RegisterState(new AIRangeRoamingState());
		StateMachine.RegisterState(new AIRangeAttackState());
		StateMachine.RegisterState(new AIRangeChaseState());
		StateMachine.RegisterState(new AIReloadState());

		StateMachine.ChangeState(InitialStateID);
	}
}

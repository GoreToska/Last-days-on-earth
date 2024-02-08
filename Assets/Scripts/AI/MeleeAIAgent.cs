using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MeleeAIAgent : BaseAIAgent
{
    public IAILightAttack AIAttack { get; private set; }

    public override void Awake()
    {
        base.Awake();

        AIAttack = GetComponent<IAILightAttack>();
    }

    public override void Start()
    {
        base.Start();

        // register all needed states
        stateMachine.RegisterState(new AIChaseState());
        stateMachine.RegisterState(new AIIdleState());
        stateMachine.RegisterState(new AIAttackState());
        stateMachine.RegisterState(new AIRoamingState());

        stateMachine.ChangeState(initialStateID);
    }

    public void SetAttackToFalse()
    {
        isAttacking = false;
    }
}

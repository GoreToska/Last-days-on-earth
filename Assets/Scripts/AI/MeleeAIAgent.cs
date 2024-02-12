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
        StateMachine.RegisterState(new AIMeleeChaseState());
        StateMachine.RegisterState(new AIIdleState());
        StateMachine.RegisterState(new AIMeleeAttackState());
        StateMachine.RegisterState(new AIRoamingState());

        StateMachine.ChangeState(InitialStateID);
    }

    public void SetAttackToFalse()
    {
        IsAttacking = false;
    }
}

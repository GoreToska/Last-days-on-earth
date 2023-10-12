using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIZombieAgent : MonoBehaviour
{
    public AIAgentConfig config;
    public Transform targetTransform;
    public AIZombieStateMachine stateMachine;
    public AIStateID initialStateID;
    public NavMeshAgent navMeshAgent;
    public Ragdoll ragdoll;
    public Animator animator;
    public bool isAttacking = false;

    private void Start()
    {
        stateMachine = new AIZombieStateMachine(this);
        navMeshAgent = GetComponent<NavMeshAgent>();
        ragdoll = GetComponent<Ragdoll>();
        animator = GetComponent<Animator>();
        targetTransform = GameObject.FindGameObjectWithTag("Player").transform;

        // register all needed states
        stateMachine.RegisterState(new AIChasePlayerState());
        stateMachine.RegisterState(new AIDeadState());
        stateMachine.RegisterState(new AIIdleState());
        stateMachine.RegisterState(new AIAttackState());

        stateMachine.ChangeState(initialStateID);
    }

    private void Update()
    {
        stateMachine.Update();
    }

    public void SetAttackToFalse()
    {
        isAttacking = false;
    }
}

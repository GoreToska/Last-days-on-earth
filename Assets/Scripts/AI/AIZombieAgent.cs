using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class AIZombieAgent : MonoBehaviour
{  
    public AIStateID initialStateID;
    public AIAgentConfig config;
    
    [HideInInspector] public Transform targetTransform;
    [HideInInspector] public AIZombieStateMachine stateMachine;
    [HideInInspector] public NavMeshAgent navMeshAgent;
    [HideInInspector] public Ragdoll ragdoll;
    [HideInInspector] public Animator animator;
    [HideInInspector] public AISensor sensor;
    [HideInInspector] public bool isAttacking = false;

    private void Start()
    {
        stateMachine = new AIZombieStateMachine(this);
        navMeshAgent = GetComponent<NavMeshAgent>();
        ragdoll = GetComponent<Ragdoll>();
        animator = GetComponent<Animator>();
        sensor = GetComponent<AISensor>();
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

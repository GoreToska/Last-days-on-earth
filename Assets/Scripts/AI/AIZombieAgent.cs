using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class AIZombieAgent : MonoBehaviour
{  
    public AIStateID initialStateID;
    public AIAgentConfig config;
    public float timeToStartRoaming = 5f;
    public float roamingRadius = 5f;
    public LayerMask groundMask;

    //[HideInInspector] public Transform targetTransform;
    [HideInInspector] public AIZombieStateMachine stateMachine;
    [HideInInspector] public NavMeshAgent navMeshAgent;
    [HideInInspector] public Ragdoll ragdoll;
    [HideInInspector] public Animator animator;
    [HideInInspector] public AISensor sensor;
    [HideInInspector] public AITargetingSystem targetSystem;
    [HideInInspector] public bool isAttacking = false;

    private void Start()
    {
        stateMachine = new AIZombieStateMachine(this);
        navMeshAgent = GetComponent<NavMeshAgent>();
        ragdoll = GetComponent<Ragdoll>();
        animator = GetComponent<Animator>();
        targetSystem = GetComponent<AITargetingSystem>();
        sensor = GetComponent<AISensor>();
        
        // register all needed states
        stateMachine.RegisterState(new AIChasePlayerState());
        stateMachine.RegisterState(new AIDeadState());
        stateMachine.RegisterState(new AIIdleState());
        stateMachine.RegisterState(new AIAttackState());
        stateMachine.RegisterState(new AIRoamingState());

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

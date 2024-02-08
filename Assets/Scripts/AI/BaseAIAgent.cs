using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BaseAIAgent : MonoBehaviour
{
    public AIStateID initialStateID;
    public AIAgentConfig config;
    public float timeToStartRoaming = 5f;
    public float roamingRadius = 5f;
    public LayerMask groundMask;

    [HideInInspector] public AIStateMachine stateMachine;
    [HideInInspector] public NavMeshAgent navMeshAgent;
    [HideInInspector] public Ragdoll ragdoll;
    [HideInInspector] public Animator animator;
    [HideInInspector] public AISensor sensor;
    [HideInInspector] public AITargetingSystem targetSystem;
    [HideInInspector] public bool isAttacking = false;

    public virtual void Awake()
    {
        stateMachine = new AIStateMachine(this);
        navMeshAgent = GetComponent<NavMeshAgent>();
        ragdoll = GetComponent<Ragdoll>();
        animator = GetComponent<Animator>();
        targetSystem = GetComponent<AITargetingSystem>();
        sensor = GetComponent<AISensor>();
    }

    public virtual void Start()
    {
        // register
        stateMachine.RegisterState(new AIDeadState());
    }

    public virtual void Update()
    {
        stateMachine.Update();
    }

    public void SetDeadState()
    {
        stateMachine.ChangeState(AIStateID.Dead);
    }

    public void AddTarget(GameObject target, float score)
    {
        Debug.Log($"Adding target {target}");
        sensor.AddTarget(target.transform.root.gameObject);
        targetSystem.AddMemory(target, score);
    }
}

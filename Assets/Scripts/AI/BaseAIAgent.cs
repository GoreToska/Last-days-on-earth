using UnityEngine;
using UnityEngine.AI;

public class BaseAIAgent : MonoBehaviour
{
    public AIStateID InitialStateID;
    public AIAgentConfig Config;
    public float TimeToStartRoaming = 5f;
    public float RoamingRadius = 5f;
    public LayerMask GroundMask;

    [HideInInspector] public AIStateMachine StateMachine;
    [HideInInspector] public NavMeshAgent NavMeshAgent;
    [HideInInspector] public Ragdoll Ragdoll;
    [HideInInspector] public Animator Animator;
    [HideInInspector] public AISensor Sensor;
    [HideInInspector] public AITargetingSystem TargetSystem;
    [HideInInspector] public bool IsAttacking = false;

    public virtual void Awake()
    {
        StateMachine = new AIStateMachine(this);
        NavMeshAgent = GetComponent<NavMeshAgent>();
        Ragdoll = GetComponent<Ragdoll>();
        Animator = GetComponent<Animator>();
        TargetSystem = GetComponent<AITargetingSystem>();
        Sensor = GetComponent<AISensor>();
    }

    public virtual void Start()
    {
        // register
        StateMachine.RegisterState(new AIDeadState());
    }

    public virtual void Update()
    {
        StateMachine.Update();
    }

    public void SetDeadState()
    {
        StateMachine.ChangeState(AIStateID.Dead);
    }

    public void AddTarget(GameObject target, float score)
    {
        Sensor.AddTarget(target.transform.root.gameObject);
        TargetSystem.AddMemory(target, score);
	}
}

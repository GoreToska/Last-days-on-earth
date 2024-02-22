using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class BaseAIAgent : MonoBehaviour
{
	public AIStateID InitialStateID;
	public AIStateID IdleStateID;
	public AIStateID ChaseStateID;
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
	[HideInInspector] public AIAnimation AIAnimation;
	[HideInInspector] public DamagableCharacter Status;

	public virtual void Awake()
	{
		StateMachine = new AIStateMachine(this);
		NavMeshAgent = GetComponent<NavMeshAgent>();
		Ragdoll = GetComponent<Ragdoll>();
		Animator = GetComponent<Animator>();
		TargetSystem = GetComponent<AITargetingSystem>();
		Sensor = GetComponent<AISensor>();
		AIAnimation = GetComponent<AIAnimation>();
		Status = GetComponent<DamagableCharacter>();
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
		StartCoroutine(DestroyObject(5));
	}

	public void AddTarget(GameObject target, float score)
	{
		Sensor.AddTarget(target);
		TargetSystem.AddMemory(target, score);

		if (StateMachine.currentStateID == IdleStateID)
			StateMachine.ChangeState(ChaseStateID);
	}

	private IEnumerator DestroyObject(float time)
	{
		yield return new WaitForSeconds(time);

		Destroy(this.gameObject);
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(Animator))]
public class AIAnimation : MonoBehaviour
{
	private NavMeshAgent _navMeshAgent;
	private Animator _animator;

	private void Start()
	{
		_navMeshAgent = GetComponent<NavMeshAgent>();
		_animator = GetComponent<Animator>();
	}

	private void Update()
	{
		if (_navMeshAgent != null && _navMeshAgent.enabled)
		{
			_animator.SetFloat("Speed", _navMeshAgent.velocity.magnitude, 0.2f, Time.deltaTime);
		}
	}

	public void PlayRifleReloadAnimation()
	{
		_animator.SetTrigger("RifleReload");
	}

	public void PlayMediumRifleShot()
	{
		_animator.SetTrigger("RIfleMediumShot");
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(Animator))]
public class AILocomotion : MonoBehaviour
{
    public Transform TargetTransform;
    private NavMeshAgent navMeshAgent;
    private Animator animator;

    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (navMeshAgent != null && navMeshAgent.enabled)
        {
            navMeshAgent.destination = TargetTransform.position;
            animator.SetFloat("Speed", navMeshAgent.velocity.magnitude, 0.2f, Time.deltaTime);
        }
    }
}

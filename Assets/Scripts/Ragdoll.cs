using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Animator))]
public class Ragdoll : MonoBehaviour
{
    private Rigidbody[] rigidbodies;
    private Animator animator;
    private NavMeshAgent navMeshAgent;

    private void Start()
    {
        rigidbodies = GetComponentsInChildren<Rigidbody>();
        animator = GetComponent<Animator>();
        TryGetComponent<NavMeshAgent>(out navMeshAgent);

        DisableRagdoll();
    }

    public void DisableRagdoll()
    {
        foreach (var rigidbody in rigidbodies)
        {
            rigidbody.isKinematic = true;
        }

        animator.enabled = true;
        if (navMeshAgent)
        {
            navMeshAgent.enabled = true;
        }
    }

    public void EnableRagdoll()
    {
        foreach (var rigidbody in rigidbodies)
        {
            rigidbody.isKinematic = false;
        }

        animator.enabled = false;
        if(navMeshAgent)
        {
            navMeshAgent.enabled = false;
        }
    }
}

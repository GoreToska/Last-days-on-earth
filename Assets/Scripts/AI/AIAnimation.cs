using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(Animator))]
public class AIAnimation : MonoBehaviour
{
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
            animator.SetFloat("Speed", navMeshAgent.velocity.magnitude, 0.2f, Time.deltaTime);
        }
    }

    public void PlayRifleReloadAnimation()
    {
        Debug.Log("Reload Rifle");
    }
}

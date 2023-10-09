using System.Collections;
using System.Collections.Generic;
using TheKiwiCoder;
using UnityEngine;
using UnityEngine.AI;

public class ZombieFOV : MonoBehaviour
{
    [Range(0, 360)]
    public float angle;
    public float radius;
    public float updateDelay = 0.2f;

    public LayerMask targetsMask;
    public LayerMask obstaclesMask;

    public bool canSeePlayer = false;

    private Vector3 CharacterPosition = Vector3.positiveInfinity;
    private Vector3 LastPosition = Vector3.positiveInfinity;

    private void Start()
    {
        StartCoroutine(FOVRoutine());
    }

    public void FOVCheck()
    {
        Collider[] rangeCheck = Physics.OverlapSphere(transform.position, radius, targetsMask);

        if (rangeCheck.Length > 0)
        {
            Transform target = rangeCheck[0].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, directionToTarget) < angle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);

                if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstaclesMask))
                {
                    canSeePlayer = true;

                    if (CharacterPosition != Vector3.positiveInfinity)
                        LastPosition =CharacterPosition;

                    CharacterPosition = target.position;
                    GetComponent<NavMeshAgent>().destination = target.position;
                }
                else
                {
                    canSeePlayer = false;
                    CharacterPosition = Vector3.positiveInfinity;
                }
            }
            else
            {
                canSeePlayer = false;
                CharacterPosition = Vector3.positiveInfinity;
            }
        }
        else
        {
            canSeePlayer = false;
            CharacterPosition = Vector3.zero;
        }
    }

    private IEnumerator FOVRoutine()
    {
        WaitForSeconds waitTime = new WaitForSeconds(updateDelay);

        while (true)
        {
            FOVCheck();

            yield return waitTime;
        }
    }
}

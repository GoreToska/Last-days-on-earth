using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class CheckForCharacter : ActionNode
{
    [SerializeField] private float FOVRadius;
    [SerializeField] private float Angle;
    [SerializeField] private float timeToWait = 0.2f;

    [SerializeField] private LayerMask targetMask;
    [SerializeField] private LayerMask obstaclesMask;

    public bool canSeeCharacter = false;

    protected override void OnStart() 
    {
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        return State.Running;
    }

    private IEnumerator FOVRoutine()
    {
        WaitForSeconds waitTime = new WaitForSeconds(timeToWait);

        while (true)
        {
            FieldOfViewCheck();

            yield return waitTime;
        }
    }

    private void FieldOfViewCheck()
    {

    }    
}

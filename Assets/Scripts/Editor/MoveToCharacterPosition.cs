using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;
using Codice.CM.Common;

public class MoveToCharacterPosition : ActionNode
{
    public float speed = 5;
    public float stoppingDistance = 0.1f;
    public bool updateRotation = true;
    public float acceleration = 40.0f;
    public float tolerance = 1.0f;

    protected override void OnStart()
    {
        context.agent.stoppingDistance = stoppingDistance;
        context.agent.speed = speed;
        context.agent.updateRotation = updateRotation;
        context.agent.acceleration = acceleration;
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        if (blackboard.ChaseCurrentPosition())
        {
            Debug.Log("Success 1");
            return State.Success;
        }
        else if (blackboard.ChaseLastPosition())
        {
            Debug.Log("Success 2");
            return State.Success;
        }
        else
        {
            Debug.Log("Fail");
            return State.Failure;
        }
    }
}

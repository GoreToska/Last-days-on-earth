using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIDeadState : AIState
{
    public void Enter(AIZombieAgent agent)
    {
        agent.ragdoll.EnableRagdoll();
    }

    public void Exit(AIZombieAgent agent)
    {
    }

    public AIStateID GetStateID()
    {
        return AIStateID.Dead;
    }

    public void Update(AIZombieAgent agent)
    {
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIDeadState : AIState
{
    public void Enter(BaseAIAgent agent)
    {
        agent.ragdoll.EnableRagdoll();
    }

    public void Exit(BaseAIAgent agent)
    {
    }

    public AIStateID GetStateID()
    {
        return AIStateID.Dead;
    }

    public void Update(BaseAIAgent agent)
    {
    }
}

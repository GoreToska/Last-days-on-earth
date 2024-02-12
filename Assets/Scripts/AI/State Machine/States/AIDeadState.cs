using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIDeadState : IAIState
{
    public void Enter(BaseAIAgent agent)
    {
        agent.Ragdoll.EnableRagdoll();
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

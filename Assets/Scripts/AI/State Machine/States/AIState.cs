using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AIStateID
{
    ChasePlayer = 0,
    Dead = 1,
    Idle = 2,
    Attack = 3,
    Roaming = 4,
}

public interface AIState
{
    AIStateID GetStateID();

    void Enter(BaseAIAgent agent);
    
    void Update(BaseAIAgent agent);

    void Exit(BaseAIAgent agent);
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AIStateID
{
    ChasePlayer = 0,
    Dead = 1,
    Idle = 2,
    MeleeAttack = 3,
    Roaming = 4,
    RangeChasePlayer = 5,
    RangeAttack = 6,
    RangeIdle = 7,
    RangeRoaming = 8,
    Reload = 9,
}

public interface IAIState
{
    AIStateID GetStateID();

    void Enter(BaseAIAgent agent);
    
    void Update(BaseAIAgent agent);

    void Exit(BaseAIAgent agent);
}

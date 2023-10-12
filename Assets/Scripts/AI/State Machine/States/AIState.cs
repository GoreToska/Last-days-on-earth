using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AIStateID
{
    ChasePlayer = 0,
    Dead = 1,
    Idle = 2,
    Attack = 3,
}

public interface AIState
{
    AIStateID GetStateID();

    void Enter(AIZombieAgent agent);
    
    void Update(AIZombieAgent agent);

    void Exit(AIZombieAgent agent);
}

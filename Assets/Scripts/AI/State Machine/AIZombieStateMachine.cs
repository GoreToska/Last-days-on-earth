using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIZombieStateMachine
{
    public AIState[] states;
    public AIZombieAgent agent;
    public AIStateID currentStateID;

    public AIZombieStateMachine(AIZombieAgent agent)
    {
        this.agent = agent;
        int numberOfStates = System.Enum.GetNames(typeof(AIStateID)).Length;
        states = new AIState[numberOfStates];
    }

    public void RegisterState(AIState state)
    {
        int index = (int)state.GetStateID();
        states[index] = state;
    }

    public void Update()
    {
        GetState(currentStateID)?.Update(agent);
    }

    public AIState GetState(AIStateID stateID)
    {
        return states[(int)stateID];
    }

    public void ChangeState(AIStateID newState)
    {
        GetState(currentStateID)?.Exit(agent);
        currentStateID = newState;
        GetState(currentStateID)?.Enter(agent);
    }
}

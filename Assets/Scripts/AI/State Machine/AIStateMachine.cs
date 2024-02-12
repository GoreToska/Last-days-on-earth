using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIStateMachine
{
    public IAIState[] states;
    public BaseAIAgent agent;
    public AIStateID currentStateID;

    public AIStateMachine(BaseAIAgent agent)
    {
        this.agent = agent;
        int numberOfStates = System.Enum.GetNames(typeof(AIStateID)).Length;
        states = new IAIState[numberOfStates];
    }

    public void RegisterState(IAIState state)
    {
        int index = (int)state.GetStateID();
        states[index] = state;
    }

    public void Update()
    {
        GetState(currentStateID)?.Update(agent);
    }

    public IAIState GetState(AIStateID stateID)
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

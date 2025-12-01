using UnityEngine;

public class IdleState : IAgentState
{
    private GOAPAgent agent;

    public IdleState(GOAPAgent agent)
    {
        this.agent = agent;
    }

    public void Enter()
    {
        Debug.Log("Agent is idle. No goals to satisfy.");
    }

    public void Exit() { }
}

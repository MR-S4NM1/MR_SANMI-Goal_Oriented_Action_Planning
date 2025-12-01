public class ReplanningState : IAgentState
{
    private GOAPAgent agent;

    public ReplanningState(GOAPAgent agent)
    {
        this.agent = agent;
    }

    public void Enter()
    {
        agent.ChangeState(new PlanningState(agent));
    }

    public void Exit() { }
}

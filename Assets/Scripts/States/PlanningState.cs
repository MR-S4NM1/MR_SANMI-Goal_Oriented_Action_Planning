public class PlanningState : IAgentState
{
    private GOAPAgent agent;

    public PlanningState(GOAPAgent agent)
    {
        this.agent = agent;
    }

    public void Enter()
    {
        var goal = agent.ChooseGoal();

        if (goal == null)
        {
            agent.ChangeState(new IdleState(agent));
            return;
        }

        var plan = agent.planner.Plan(agent.worldState, agent.actions, goal);

        if (plan == null)
        {
            agent.ChangeState(new IdleState(agent));
            return;
        }

        agent.CurrentPlan = plan;
        agent.ChangeState(new ExecutingPlanState(agent));
    }

    public void Exit() { }
}

using UnityEngine;

public class ExecutingPlanState : IAgentState
{
    private GOAPAgent agent;

    public ExecutingPlanState(GOAPAgent agent)
    {
        this.agent = agent;
    }

    public void Enter()
    {
        ExecuteNext();
    }

    private void ExecuteNext()
    {
        if (agent.CurrentPlan == null || agent.CurrentPlan.Count == 0)
        {
            agent.ChangeState(new PlanningState(agent));
            return;
        }

        var action = agent.CurrentPlan.Peek();
        action.OnActionCompleted += OnActionDone;
        action.Execute(agent.worldState);
    }

    private void OnActionDone(GOAPAction action)
    {
        action.OnActionCompleted -= OnActionDone;

        if (agent.CurrentPlan != null && agent.CurrentPlan.Count > 0)
            agent.CurrentPlan.Dequeue();

        if (agent.CurrentPlan == null || agent.CurrentPlan.Count == 0)
        {
            agent.ChangeState(new PlanningState(agent));
            return;
        }

        ExecuteNext();
    }

    public void Exit() { }
}

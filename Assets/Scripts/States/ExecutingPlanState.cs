using System.Collections;

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

        agent.currentAction = action;

        if (agent.currentActionRoutine != null)
        {
            agent.StopCoroutine(agent.currentActionRoutine);
            agent.currentActionRoutine = null;
        }

        action.OnActionCompleted += OnActionDone;

        agent.currentActionRoutine = agent.StartCoroutine(ActionWrapper(action));
    }

    private IEnumerator ActionWrapper(GOAPAction action)
    {
        agent.currentActionRoutine = agent.StartCoroutine(
            agent.currentAction.ExecuteRoutine(agent.worldState));
        yield return null;
    }

    private void OnActionDone(GOAPAction action)
    {
        action.OnActionCompleted -= OnActionDone;

        agent.currentAction = null;
        agent.currentActionRoutine = null;

        if (agent.CurrentPlan != null && agent.CurrentPlan.Count > 0)
            agent.CurrentPlan.Dequeue();

        if (agent.CurrentPlan == null || agent.CurrentPlan.Count == 0)
        {
            agent.ChangeState(new PlanningState(agent));
            return;
        }

        ExecuteNext();
    }

    public void Exit()
    {
        if (agent.currentActionRoutine != null)
        {
            agent.StopCoroutine(agent.currentActionRoutine);
            agent.currentActionRoutine = null;
        }

        if (agent.currentAction != null)
        {
            agent.currentAction.Cancel();
            agent.currentAction = null;
        }
    }
}

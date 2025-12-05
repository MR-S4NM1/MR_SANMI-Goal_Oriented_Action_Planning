using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class PlanningState : IAgentState
{
    private GOAPAgent agent;

    public PlanningState(GOAPAgent agent)
    {
        this.agent = agent;
    }

    public void Enter()
    {
        if (agent.worldState.ContainsKey("TownInDanger") &&
            (bool)agent.worldState["TownInDanger"] == true)
        {
            var catchGoal = agent.goals.Find(g => g.name == "CatchThief");
            if (catchGoal != null)
            {
                var plan = agent.planner.Plan(agent.worldState, agent.actions, catchGoal);
                if (plan != null && plan.Count > 0)
                {
                    agent.CurrentPlan = plan;
                    agent.ChangeState(new ExecutingPlanState(agent));
                    return;
                }
            }
        }

        var orderedGoals = agent.goals
            .OrderByDescending(g => g.priority)
            .ToList();

        Queue<GOAPAction> chosenPlan = null;
        GOAPGoal chosenGoal = null;

        foreach (var goal in orderedGoals)
        {
            var plan = agent.planner.Plan(agent.worldState, agent.actions, goal);

            if (plan == null)
            {
                Debug.Log($"[Planning] No plan for goal '{goal.name}' (priority {goal.priority}). Trying next goal...");
                continue;
            }

            if (plan.Count == 0)
            {
                Debug.Log($"[Planning] Goal '{goal.name}' is already satisfied. Removing from goal list.");
                continue;
            }

            chosenPlan = plan;
            chosenGoal = goal;
            Debug.Log($"[Planning] Chosen goal '{goal.name}' with priority {goal.priority}. Plan length: {plan.Count}");
            break;
        }

        if (chosenPlan == null)
        {
            Debug.LogWarning("[Planning] No valid plans found for any goal. Switching to Idle.");
            agent.ChangeState(new IdleState(agent));
            return;
        }

        agent.CurrentPlan = chosenPlan;
        agent.ChangeState(new ExecutingPlanState(agent));
    }


    public void Exit() { }
}

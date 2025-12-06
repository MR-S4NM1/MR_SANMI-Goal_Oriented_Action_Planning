using System.Linq;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Planning state where the AI agent evaluates goals and generates action plans.
/// Implements goal prioritization and emergency response logic.
/// </summary>
/// <remarks>
/// Author: Miguel Angel Garcia Elizalde
/// Algorithm: Goal selection with priority sorting
/// Optimization: Emergency goal bypass for critical situations
/// Complexity: O(n log n) for goal sorting + O(m) for planning
/// </remarks>
public class PlanningState : IAgentState
{
    private GOAPAgent agent;

    /// <summary>
    /// Initializes a new PlanningState for the specified agent.
    /// </summary>
    /// <param name="agent">The agent that will perform planning.</param>
    public PlanningState(GOAPAgent agent)
    {
        this.agent = agent;
    }

    /// <summary>
    /// Enters the planning state. Performs goal evaluation and plan generation.
    /// </summary>
    /// <remarks>
    /// Execution flow:
    /// 1. Check for emergency situations (TownInDanger)
    /// 2. Sort goals by priority (descending)
    /// 3. Attempt to plan for each goal until success
    /// 4. Transition to execution or idle state
    /// </remarks>
    public void Enter()
    {
        Debug.Log($"[{agent.name}] Entered PlanningState");

        // Emergency response: Handle critical world state changes immediately
        if (ShouldRespondToEmergency())
        {
            HandleEmergencyResponse();
            return;
        }

        // Normal planning: Evaluate all goals by priority
        ExecuteNormalPlanning();
    }

    /// <summary>
    /// Checks if the agent should respond to an emergency situation.
    /// </summary>
    /// <returns>True if an emergency condition is active.</returns>
    private bool ShouldRespondToEmergency()
    {
        return agent.worldState.ContainsKey("TownInDanger") &&
               (bool)agent.worldState["TownInDanger"] == true;
    }

    /// <summary>
    /// Handles emergency response planning for critical situations.
    /// </summary>
    private void HandleEmergencyResponse()
    {
        Debug.Log($"[{agent.name}] Emergency detected! Forcing CatchThief goal.");

        var catchGoal = agent.goals.Find(g => g.name == "CatchThief");
        if (catchGoal == null)
        {
            Debug.LogWarning($"[{agent.name}] No CatchThief goal found for emergency response.");
            ExecuteNormalPlanning();
            return;
        }

        var emergencyPlan = agent.planner.Plan(agent.worldState, agent.actions, catchGoal);
        if (emergencyPlan != null && emergencyPlan.Count > 0)
        {
            Debug.Log($"[{agent.name}] Emergency plan generated with {emergencyPlan.Count} actions");
            agent.CurrentPlan = emergencyPlan;
            agent.ChangeState(new ExecutingPlanState(agent));
        }
        else
        {
            Debug.LogWarning($"[{agent.name}] Failed to generate emergency plan. Falling back to normal planning.");
            ExecuteNormalPlanning();
        }
    }

    /// <summary>
    /// Executes normal goal-based planning in priority order.
    /// </summary>
    private void ExecuteNormalPlanning()
    {
        // Sort goals by priority (highest first)
        var prioritizedGoals = agent.goals
            .OrderByDescending(g => g.priority)
            .ToList();

        Queue<GOAPAction> selectedPlan = null;
        GOAPGoal selectedGoal = null;

        // Iterate through goals in priority order
        foreach (var goal in prioritizedGoals)
        {
            Debug.Log($"[{agent.name}] Planning for goal: {goal.name} (Priority: {goal.priority})");

            var plan = agent.planner.Plan(agent.worldState, agent.actions, goal);

            if (plan == null)
            {
                Debug.Log($"[{agent.name}] No valid plan found for goal '{goal.name}'");
                continue;
            }

            if (plan.Count == 0)
            {
                Debug.Log($"[{agent.name}] Goal '{goal.name}' is already satisfied");
                continue;
            }

            selectedPlan = plan;
            selectedGoal = goal;
            Debug.Log($"[{agent.name}] Selected goal '{goal.name}' with {plan.Count} actions");
            break;
        }

        // State transition based on planning outcome
        if (selectedPlan == null)
        {
            Debug.LogWarning($"[{agent.name}] No achievable goals found. Transitioning to Idle.");
            agent.ChangeState(new IdleState(agent));
        }
        else
        {
            agent.CurrentPlan = selectedPlan;
            agent.ChangeState(new ExecutingPlanState(agent));
        }
    }

    /// <summary>
    /// Exits the planning state. No special cleanup needed.
    /// </summary>
    public void Exit()
    {
        // Cleanup any planning resources if needed
    }
}
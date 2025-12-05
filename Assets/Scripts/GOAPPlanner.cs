using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Core planner that generates action sequences to achieve AI goals.
/// Implements forward-chaining search with heuristic action selection.
/// </summary>
/// <remarks>
/// Author: Marco Antonio Garcia.
/// Algorithm: Forward state-space planning with heuristics
/// Complexity: O(n * m) where n = actions, m = planning depth
/// Optimization: Early termination, action pruning
/// </remarks>

public class GOAPPlanner
{
    /// <summary>
    /// Maximum planning iterations to prevent infinite loops.
    /// </summary>
    private const int MAX_PLANNING_ITERATIONS = 50;

    /// <summary>
    /// Generates a sequence of actions to achieve a specified goal.
    /// </summary>
    /// <param name="worldState">Current global world state.</param>
    /// <param name="availableActions">Actions the agent can perform.</param>
    /// <param name="goal">The goal to achieve.</param>
    /// <returns>Queue of actions to execute, or null if no plan found.</returns>
    public Queue<GOAPAction> Plan(
        WorldState worldState,
        List<GOAPAction> availableActions,
        GOAPGoal goal)
    {
        // Create simulated state for planning
        WorldState simulatedState = CloneWorldState(worldState);
        List<GOAPAction> plan = new List<GOAPAction>();

        int iteration = 0;

        // Main planning loop
        while (!IsGoalSatisfied(simulatedState, goal) && iteration < MAX_PLANNING_ITERATIONS)
        {
            iteration++;

            GOAPAction selectedAction = SelectBestAction(
                simulatedState,
                availableActions,
                goal
            );

            if (selectedAction == null)
            {
                Debug.LogWarning($"[GOAPPlanner] No applicable actions found at iteration {iteration}");
                return null;
            }

            // Apply action effects to simulated state
            ApplyActionEffects(selectedAction, simulatedState);
            plan.Add(selectedAction);
        }

        // Validate final plan
        if (!IsGoalSatisfied(simulatedState, goal))
        {
            Debug.LogWarning($"[GOAPPlanner] Failed to satisfy goal '{goal.name}' after {iteration} iterations");
            return null;
        }

        Debug.Log($"[GOAPPlanner] Generated plan with {plan.Count} actions for goal '{goal.name}'");
        return new Queue<GOAPAction>(plan);
    }

    /// <summary>
    /// Creates a deep copy of the world state for simulation.
    /// </summary>
    private WorldState CloneWorldState(WorldState original)
    {
        WorldState clone = new WorldState();
        foreach (var kvp in original)
        {
            clone[kvp.Key] = kvp.Value;
        }
        return clone;
    }

    /// <summary>
    /// Selects the best action based on heuristic evaluation.
    /// </summary>
    private GOAPAction SelectBestAction(
        WorldState currentState,
        List<GOAPAction> actions,
        GOAPGoal goal)
    {
        GOAPAction bestAction = null;
        bool foundDirectImprovement = false;
        float bestCost = float.MaxValue;

        foreach (var action in actions)
        {
            // 1. Check preconditions
            if (!action.ArePreconditionsMet(currentState))
                continue;

            // 2. Check if action changes anything
            if (!ActionChangesState(action, currentState))
                continue;

            // 3. Evaluate action against goal
            bool improvesGoal = ActionImprovesGoal(action, goal);

            // 4. Selection heuristics:
            //    - Prioritize actions that directly improve the goal
            //    - Among equal candidates, choose lowest cost
            if (improvesGoal)
            {
                if (!foundDirectImprovement || action.cost < bestCost)
                {
                    bestAction = action;
                    bestCost = action.cost;
                    foundDirectImprovement = true;
                }
            }
            else if (!foundDirectImprovement)
            {
                if (action.cost < bestCost)
                {
                    bestAction = action;
                    bestCost = action.cost;
                }
            }
        }

        return bestAction;
    }

    /// <summary>
    /// Checks if an action would change the current world state.
    /// </summary>
    private bool ActionChangesState(GOAPAction action, WorldState state)
    {
        foreach (var effect in action.Effects)
        {
            if (!state.ContainsKey(effect.Key) ||
                !state[effect.Key].Equals(effect.Value))
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Checks if an action directly contributes to goal satisfaction.
    /// </summary>
    private bool ActionImprovesGoal(GOAPAction action, GOAPGoal goal)
    {
        foreach (var effect in action.Effects)
        {
            if (goal.desiredState.ContainsKey(effect.Key) &&
                goal.desiredState[effect.Key].Equals(effect.Value))
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Applies an action's effects to a world state.
    /// </summary>
    private void ApplyActionEffects(GOAPAction action, WorldState state)
    {
        foreach (var effect in action.Effects)
        {
            state[effect.Key] = effect.Value;
        }
    }

    /// <summary>
    /// Checks if a goal is satisfied by the current world state.
    /// </summary>
    private bool IsGoalSatisfied(WorldState state, GOAPGoal goal)
    {
        foreach (var condition in goal.desiredState)
        {
            if (!state.ContainsKey(condition.Key) ||
                !state[condition.Key].Equals(condition.Value))
            {
                return false;
            }
        }
        return true;
    }
}
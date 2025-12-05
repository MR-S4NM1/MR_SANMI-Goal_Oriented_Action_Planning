using System.Collections;
using UnityEngine;

/// <summary>
/// Execution state where the AI agent carries out its planned actions.
/// Manages action sequencing, completion callbacks, and error handling.
/// </summary>
/// <remarks>
/// Author: Miguel Angel Garcia Elizalde
/// Pattern: Command pattern for action execution
/// Features: Action cancellation, sequential execution, event handling
/// Error Handling: Graceful failure recovery via replanning
/// </remarks>

public class ExecutingPlanState : IAgentState
{
    private GOAPAgent agent;

    /// <summary>
    /// Initializes a new ExecutingPlanState for the specified agent.
    /// </summary>
    /// <param name="agent">The agent that will execute the plan.</param>
    public ExecutingPlanState(GOAPAgent agent)
    {
        this.agent = agent;
    }

    /// <summary>
    /// Enters the execution state and starts executing the first action.
    /// </summary>
    public void Enter()
    {
        Debug.Log($"[{agent.name}] Entered ExecutingPlanState");
        ExecuteNextAction();
    }

    /// <summary>
    /// Executes the next action in the current plan.
    /// Handles empty plan conditions and transitions back to planning.
    /// </summary>
    private void ExecuteNextAction()
    {
        // Check for plan completion or invalidity
        if (agent.CurrentPlan == null || agent.CurrentPlan.Count == 0)
        {
            Debug.Log($"[{agent.name}] Plan completed or invalid. Returning to planning.");
            agent.ChangeState(new PlanningState(agent));
            return;
        }

        // Peek at next action without removing it from queue
        var nextAction = agent.CurrentPlan.Peek();
        Debug.Log($"[{agent.name}] Executing action: {nextAction.name}");

        // Set current action reference
        agent.currentAction = nextAction;

        // Clean up any previous action execution
        CleanupPreviousAction();

        // Subscribe to action completion event
        nextAction.OnActionCompleted += OnActionCompleted;

        // Start action execution coroutine
        agent.currentActionRoutine = agent.StartCoroutine(
            ExecuteActionCoroutine(nextAction)
        );
    }

    /// <summary>
    /// Cleans up resources from previous action execution.
    /// </summary>
    private void CleanupPreviousAction()
    {
        if (agent.currentActionRoutine != null)
        {
            agent.StopCoroutine(agent.currentActionRoutine);
            agent.currentActionRoutine = null;
        }
    }

    /// <summary>
    /// Coroutine wrapper for action execution with proper error handling.
    /// </summary>
    /// <param name="action">The action to execute.</param>
    private IEnumerator ExecuteActionCoroutine(GOAPAction action)
    {
        Debug.Log($"[{agent.name}] Starting action coroutine: {action.name}");

        // Execute the action and yield control
        agent.currentActionRoutine = agent.StartCoroutine(
            action.ExecuteRoutine(agent.worldState)
        );

        yield return agent.currentActionRoutine;

        Debug.Log($"[{agent.name}] Action coroutine completed: {action.name}");
    }

    /// <summary>
    /// Callback invoked when an action completes execution.
    /// </summary>
    /// <param name="action">The completed action.</param>
    private void OnActionCompleted(GOAPAction action)
    {
        Debug.Log($"[{agent.name}] Action completed: {action.name}");

        // Unsubscribe from event to prevent memory leaks
        action.OnActionCompleted -= OnActionCompleted;

        // Clean up current action references
        agent.currentAction = null;
        agent.currentActionRoutine = null;

        // Remove completed action from plan
        if (agent.CurrentPlan != null && agent.CurrentPlan.Count > 0)
        {
            agent.CurrentPlan.Dequeue();
        }

        // Check if plan is complete
        if (agent.CurrentPlan == null || agent.CurrentPlan.Count == 0)
        {
            Debug.Log($"[{agent.name}] All actions completed. Returning to planning.");
            agent.ChangeState(new PlanningState(agent));
            return;
        }

        // Execute next action in sequence
        ExecuteNextAction();
    }

    /// <summary>
    /// Exits the execution state, ensuring proper cleanup of ongoing actions.
    /// </summary>
    /// <remarks>
    /// Critical for preventing orphaned coroutines and action references.
    /// </remarks>
    public void Exit()
    {
        Debug.Log($"[{agent.name}] Exiting ExecutingPlanState");

        // Stop any running coroutines
        if (agent.currentActionRoutine != null)
        {
            agent.StopCoroutine(agent.currentActionRoutine);
            agent.currentActionRoutine = null;
        }

        // Cancel current action if it exists
        if (agent.currentAction != null)
        {
            agent.currentAction.Cancel();

            // Unsubscribe from event to prevent memory leaks
            agent.currentAction.OnActionCompleted -= OnActionCompleted;

            agent.currentAction = null;
        }
    }
}
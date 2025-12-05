using UnityEngine;

/// <summary>
/// Replanning state that forces immediate plan regeneration.
/// Used as a transition state when world conditions change unexpectedly.
/// </summary>
/// <remarks>
/// Author: Miguel Angel Garcia Elizalde
/// Purpose: Clean state transition for reactive planning
/// Usage: Triggered by WorldState change events
/// Optimization: Minimal overhead, immediate transition
/// </remarks>
public class ReplanningState : IAgentState
{
    private GOAPAgent agent;

    /// <summary>
    /// Initializes a new ReplanningState for the specified agent.
    /// </summary>
    /// <param name="agent">The agent that needs to replan.</param>
    public ReplanningState(GOAPAgent agent)
    {
        this.agent = agent;
    }

    /// <summary>
    /// Enters the replanning state, immediately transitioning to planning.
    /// </summary>
    /// <remarks>
    /// This state exists to ensure clean transitions when the world state
    /// changes during execution, forcing agents to reconsider their plans.
    /// </remarks>
    public void Enter()
    {
        Debug.Log($"[{agent.name}] Entered ReplanningState - Forcing plan regeneration");
        agent.ChangeState(new PlanningState(agent));
    }

    /// <summary>
    /// Exits the replanning state. No cleanup needed as this is a transient state.
    /// </summary>
    public void Exit()
    {
        // Transient state - no persistent resources to clean
    }
}
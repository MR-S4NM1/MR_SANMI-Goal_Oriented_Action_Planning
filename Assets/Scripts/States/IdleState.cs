using UnityEngine;

/// <summary>
/// Idle state for AI agents when no goals are available or achievable.
/// Represents a resting/waiting state in the agent's behavior cycle.
/// </summary>
/// <remarks>
/// Author: Miguel Angel Garcia Elizalde
/// State: Terminal when no goals exist
/// Transition: PlanningState when goals become available
/// </remarks>
public class IdleState : IAgentState
{
    private GOAPAgent agent;

    /// <summary>
    /// Initializes a new IdleState for the specified agent.
    /// </summary>
    /// <param name="agent">The agent that will be in this state.</param>
    public IdleState(GOAPAgent agent)
    {
        this.agent = agent;
    }

    /// <summary>
    /// Enters the idle state. Logs current status and can be extended
    /// for idle animations or behaviors.
    /// </summary>
    public void Enter()
    {
        Debug.Log($"[{agent.name}] Entered IdleState - No active goals to satisfy.");
        // Optional: Trigger idle animation
        // agent.animationsManager?.PlayIdleAnimation();
    }

    /// <summary>
    /// Exits the idle state. No special cleanup needed for this state.
    /// </summary>
    public void Exit()
    {
        // Optional cleanup if idle behaviors were started
    }
}
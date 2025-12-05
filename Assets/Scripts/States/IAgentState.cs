/// <summary>
/// Interface defining the contract for AI agent states in the State Machine pattern.
/// </summary>
/// <remarks>
/// Author: Miguel Angel Garcia Elizalde
/// Pattern: State
/// Usage: All concrete state implementations must inherit this interface
/// </remarks>
public interface IAgentState
{
    /// <summary>
    /// Called when entering this state. Perform initialization here.
    /// </summary>
    void Enter();

    /// <summary>
    /// Called when exiting this state. Perform cleanup here.
    /// </summary>
    void Exit();
}
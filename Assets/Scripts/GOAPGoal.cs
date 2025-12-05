/// <summary>
/// Defines an AI goal with desired world state conditions and priority.
/// </summary>
/// <remarks>
/// Author: Miguel Angel Garcia Elizalde and Marco Antonio Garcia
/// Structure: Immutable after creation
/// Usage: Created by agent-specific implementations
/// </remarks>
using System.Collections.Generic;

[System.Serializable]
public class GOAPGoal
{
    /// <summary>
    /// Unique identifier for the goal.
    /// </summary>
    public string name;

    /// <summary>
    /// Priority value for goal selection (higher = more important).
    /// </summary>
    public float priority;

    /// <summary>
    /// Dictionary of world state conditions required to satisfy this goal.
    /// </summary>
    public Dictionary<string, object> desiredState = new Dictionary<string, object>();

    /// <summary>
    /// Constructs a new GOAP goal.
    /// </summary>
    /// <param name="name">Goal identifier.</param>
    /// <param name="priority">Selection priority weight.</param>
    public GOAPGoal(string name, float priority)
    {
        this.name = name;
        this.priority = priority;
    }

    /// <summary>
    /// Adds a condition to this goal's desired state.
    /// </summary>
    /// <param name="key">World state key.</param>
    /// <param name="value">Desired value.</param>
    public void AddDesiredState(string key, object value)
    {
        desiredState[key] = value;
    }
}
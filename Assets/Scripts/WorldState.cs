using System;
using System.Collections.Generic;

/// <summary>
/// Represents the global world state shared between all AI agents.
/// Implements the Observer pattern to notify agents of state changes.
/// </summary>
/// <remarks>
/// Author: Miguel Angel Garcia Elizalde and Marco Antonio Garcia
/// System: Goal-Oriented Action Planning (GOAP)
/// Version: 1.0
/// </remarks>
public class WorldState : Dictionary<string, object>
{
    /// <summary>
    /// Event triggered when any world state value changes.
    /// Parameters: (string key, object newValue)
    /// </summary>
    public event Action<string, object> OnWorldStateChanged;

    /// <summary>
    /// Indexer override to trigger change events on state modifications.
    /// </summary>
    /// <param name="key">The world state key to access.</param>
    /// <returns>The current value of the world state key.</returns>
    public new object this[string key]
    {
        get => base[key];
        set
        {
            // Only trigger event if value actually changed
            if (!ContainsKey(key) || !base[key].Equals(value))
            {
                base[key] = value;
                OnWorldStateChanged?.Invoke(key, value);
            }
        }
    }
}

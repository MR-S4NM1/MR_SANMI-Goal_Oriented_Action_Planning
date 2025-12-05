/// <summary>
/// Singleton manager for the global world state.
/// Ensures all AI agents share and react to the same world state.
/// </summary>
/// <remarks>
/// Author: Miguel Angel Garcia Elizalde
/// Pattern: Singleton
/// Usage: Add to a GameObject in the scene
/// </remarks>
using UnityEngine;

[DisallowMultipleComponent]
public class GlobalWorldState : MonoBehaviour
{
    /// <summary>
    /// Singleton instance accessor.
    /// </summary>
    public static GlobalWorldState Instance { get; private set; }

    /// <summary>
    /// The shared world state accessible by all agents.
    /// </summary>
    public WorldState State { get; private set; }

    /// <summary>
    /// Initializes the singleton and creates the world state.
    /// </summary>
    private void Awake()
    {
        // Singleton pattern implementation
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
        State = new WorldState();

        // Persist across scene loads
        DontDestroyOnLoad(this.gameObject);

        Debug.Log($"[GlobalWorldState] Initialized!");
    }
}
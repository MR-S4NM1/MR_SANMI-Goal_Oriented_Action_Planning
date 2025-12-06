using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Abstract base class for all GOAP actions. Defines the contract for actionable behaviors
/// that AI agents can perform as part of their plans.
/// </summary>
/// <remarks>
/// Author: Miguel Angel Garcia Elizalde and Marco Antonio Garcia
/// Design Pattern: Template Method
/// Responsibilities:
/// - Precondition validation
/// - Effect application
/// - Action execution with cancellation support
/// - Animation integration
/// - World state modification
/// </remarks>
public abstract class GOAPAction : MonoBehaviour
{
    /// <summary>
    /// Flag indicating if this action has been cancelled during execution.
    /// </summary>
    protected bool isCancelled = false;

    /// <summary>
    /// Cost value used by planner for action selection. Lower cost = preferred action.
    /// </summary>
    [Tooltip("Action cost for planner heuristic. Lower values are preferred.")]
    public float cost = 1f;

    /// <summary>
    /// Preconditions required for this action to be executable.
    /// Key: World state variable, Value: Required value
    /// </summary>
    protected Dictionary<string, object> preconditions = new Dictionary<string, object>();

    /// <summary>
    /// Effects this action will apply to the world state upon completion.
    /// Key: World state variable, Value: New value
    /// </summary>
    protected Dictionary<string, object> effects = new Dictionary<string, object>();

    /// <summary>
    /// Event triggered when this action completes execution successfully.
    /// </summary>
    public event System.Action<GOAPAction> OnActionCompleted;

    /// <summary>
    /// Public accessor for action preconditions (read-only).
    /// </summary>
    public Dictionary<string, object> Preconditions => preconditions;

    /// <summary>
    /// Public accessor for action effects (read-only).
    /// </summary>
    public Dictionary<string, object> Effects => effects;

    /// <summary>
    /// Reference to the animation manager for action-animation synchronization.
    /// </summary>
    [SerializeField]
    [Tooltip("Animation manager for action-animation synchronization.")]
    protected AnimationsManager animationsManager;

    /// <summary>
    /// Duration of the current animation being played by this action.
    /// </summary>
    protected float currentAnimationDuration;

    /// <summary>
    /// Adds a precondition to this action.
    /// </summary>
    /// <param name="key">World state variable name.</param>
    /// <param name="value">Required value for the variable.</param>
    protected void AddPrecondition(string key, object value) => preconditions[key] = value;

    /// <summary>
    /// Adds an effect to this action.
    /// </summary>
    /// <param name="key">World state variable name.</param>
    /// <param name="value">New value to set for the variable.</param>
    protected void AddEffect(string key, object value) => effects[key] = value;

    /// <summary>
    /// Unity Awake method. Ensures animation manager reference is valid.
    /// </summary>
    private void Awake()
    {
        if (animationsManager == null)
            animationsManager = GetComponent<AnimationsManager>();
    }

    /// <summary>
    /// Validates if all preconditions are met by the current world state.
    /// </summary>
    /// <param name="state">The world state to validate against.</param>
    /// <returns>True if all preconditions are satisfied.</returns>
    public bool ArePreconditionsMet(WorldState state)
    {
        foreach (var p in preconditions)
        {
            if (!state.ContainsKey(p.Key))
                return false;

            if (!state[p.Key].Equals(p.Value))
                return false;
        }

        return true;
    }

    /// <summary>
    /// Main execution coroutine wrapper. Resets cancellation flag and delegates to concrete implementation.
    /// </summary>
    /// <param name="state">The world state to operate on.</param>
    /// <returns>IEnumerator for coroutine execution.</returns>
    public IEnumerator ExecuteRoutine(WorldState state)
    {
        isCancelled = false;
        yield return PerformAction(state);
    }

    /// <summary>
    /// Cancels this action's execution. Sets cancellation flag for interruption handling.
    /// </summary>
    /// <remarks>
    /// Override in derived classes for custom cancellation cleanup.
    /// </remarks>
    public virtual void Cancel()
    {
        isCancelled = true;
        Debug.LogWarning($"[GOAPAction] {name} was cancelled! (Cancel() called)");
    }

    /// <summary>
    /// Abstract method defining the concrete action behavior.
    /// Must be implemented by all derived action classes.
    /// </summary>
    /// <param name="state">The world state to operate on.</param>
    /// <returns>IEnumerator for coroutine execution.</returns>
    protected abstract IEnumerator PerformAction(WorldState state);

    /// <summary>
    /// Completes the action execution. Applies effects to global world state and triggers completion event.
    /// </summary>
    /// <param name="state">The world state to update.</param>
    protected void Complete(WorldState state)
    {
        // Apply all effects to the global world state
        foreach (var e in effects)
        {
            GlobalWorldState.Instance.State[e.Key] = e.Value;
        }

        // Notify subscribers of completion
        OnActionCompleted?.Invoke(this);
    }
}
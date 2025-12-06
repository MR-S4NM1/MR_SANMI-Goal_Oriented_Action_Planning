using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for all AI agents using the Goal-Oriented Action Planning (GOAP) system.
/// Provides core functionality for goal management, planning, execution, and reactive behavior.
/// </summary>
/// <remarks>
/// Author: Miguel Angel Garcia Elizalde and Marco Antonio Garcia
/// Design Pattern: Template Method
/// Responsibilities:
/// - State machine management
/// - Goal evaluation and planning
/// - Action execution
/// - Reactive event handling
/// - World state synchronization
/// </remarks>
[RequireComponent(typeof(AnimationsManager))]
public class GOAPAgent : MonoBehaviour
{
    /// <summary>
    /// List of actions this agent can perform. Populated automatically from attached components.
    /// </summary>
    [Tooltip("Actions available to this agent. Auto-populated from attached GOAPAction components.")]
    public List<GOAPAction> actions = new List<GOAPAction>();

    /// <summary>
    /// Goals this agent can pursue, ordered by priority.
    /// </summary>
    [Tooltip("Agent's goals with priority values. Higher priority = more important.")]
    public List<GOAPGoal> goals = new List<GOAPGoal>();

    /// <summary>
    /// The planner instance used to generate action sequences for goals.
    /// </summary>
    [Tooltip("GOAP planner instance for generating action plans.")]
    public GOAPPlanner planner = new GOAPPlanner();

    /// <summary>
    /// Flag indicating whether the agent has completed initialization.
    /// </summary>
    [Tooltip("Indicates if agent initialization is complete.")]
    public bool isInitialized = false;

    /// <summary>
    /// Reference to the currently executing action.
    /// </summary>
    [Tooltip("Action currently being executed.")]
    public GOAPAction currentAction;

    /// <summary>
    /// Coroutine reference for the currently executing action.
    /// </summary>
    [Tooltip("Coroutine handling current action execution.")]
    public Coroutine currentActionRoutine;

    /// <summary>
    /// Current action plan being executed.
    /// </summary>
    [Tooltip("Queue of actions in the current plan.")]
    public Queue<GOAPAction> CurrentPlan { get; set; }

    /// <summary>
    /// Accessor for the global world state singleton.
    /// </summary>
    [Tooltip("Reference to shared global world state.")]
    public WorldState worldState => GlobalWorldState.Instance.State;

    /// <summary>
    /// Set of world state keys that trigger reactive replanning when changed.
    /// </summary>
    [Tooltip("World state keys that trigger agent reactions when changed.")]
    public HashSet<string> reactiveKeys = new HashSet<string>();

    /// <summary>
    /// Reference to the current agent state in the state machine.
    /// </summary>
    protected IAgentState currentState;

    /// <summary>
    /// Unity Start method. Begins delayed initialization.
    /// </summary>
    void Start()
    {
        StartCoroutine(DelayedInitialize());
    }

    /// <summary>
    /// Delayed initialization to prevent race conditions in complex scenes.
    /// Waits 2 frames before initializing to ensure all components are ready.
    /// </summary>
    protected IEnumerator DelayedInitialize()
    {
        // Wait 2 frames for robust initialization in complex scenes
        yield return null;
        yield return null;

        // Safety check: prevent duplicate initialization
        if (!isInitialized)
            InitializeAgent();
    }

    /// <summary>
    /// Main initialization method for the agent. Sets up world state, actions, goals, and event subscriptions.
    /// </summary>
    /// <remarks>
    /// Initialization order:
    /// 1. Prepare world state
    /// 2. Collect available actions
    /// 3. Set up goals
    /// 4. Subscribe to world state changes
    /// 5. Enter initial planning state
    /// </remarks>
    protected virtual void InitializeAgent()
    {
        if (isInitialized) return;

        Debug.Log($"[{name}] Initializing GOAPAgent");

        // 1. Prepare agent-specific world state
        PrepareWorldState();

        // 2. Collect all GOAPAction components attached to this GameObject
        actions.Clear();
        actions.AddRange(GetComponents<GOAPAction>());
        Debug.Log($"[{name}] Found {actions.Count} actions");

        // 3. Set up agent goals
        PrepareGoals();
        Debug.Log($"[{name}] Prepared {goals.Count} goals");

        // 4. Subscribe to relevant world state changes
        GlobalWorldState.Instance.State.OnWorldStateChanged += OnWorldStateChanged;

        // 5. Mark as initialized and start planning
        isInitialized = true;
        ChangeState(new PlanningState(this));

        Debug.Log($"[{name}] Initialization complete");
    }

    /// <summary>
    /// Cleanup method called when the GameObject is destroyed.
    /// Unsubscribes from world state events to prevent memory leaks.
    /// </summary>
    private void OnDestroy()
    {
        if (GlobalWorldState.Instance != null)
        {
            GlobalWorldState.Instance.State.OnWorldStateChanged -= OnWorldStateChanged;
            Debug.Log($"[{name}] Unsubscribed from world state events");
        }
    }

    /// <summary>
    /// Template method for setting up agent-specific world state values.
    /// Override in derived classes to define initial state.
    /// </summary>
    /// <remarks>
    /// Called during initialization before event subscriptions.
    /// Use to set up local and global state defaults.
    /// </remarks>
    protected virtual void PrepareWorldState()
    {
        // Base implementation - override in derived classes
        Debug.Log($"[{name}] Base PrepareWorldState called");
    }

    /// <summary>
    /// Template method for setting up agent goals and priorities.
    /// Override in derived classes to define agent objectives.
    /// </summary>
    /// <remarks>
    /// Called during initialization after world state setup.
    /// Define goals with priority values and desired state conditions.
    /// </remarks>
    protected virtual void PrepareGoals()
    {
        // Base implementation - override in derived classes
        Debug.Log($"[{name}] Base PrepareGoals called");
    }

    /// <summary>
    /// Changes the agent's current state, handling proper state transitions.
    /// </summary>
    /// <param name="newState">The new state to transition to.</param>
    /// <remarks>
    /// Ensures proper cleanup of previous state before entering new state.
    /// Implements the State pattern for agent behavior management.
    /// </remarks>
    public virtual void ChangeState(IAgentState newState)
    {
        Debug.Log($"[{name}] State change: {currentState?.GetType().Name} -> {newState?.GetType().Name}");

        // Exit current state if exists
        currentState?.Exit();

        // Update state reference
        currentState = newState;

        // Enter new state
        currentState?.Enter();
    }

    /// <summary>
    /// Selects the most appropriate goal based on priority and current world state.
    /// </summary>
    /// <returns>The highest priority unsatisfied goal, or null if all goals are satisfied.</returns>
    /// <remarks>
    /// Used by the planning system to determine which goal to pursue.
    /// Considers both priority and current satisfaction state.
    /// </remarks>
    public GOAPGoal ChooseGoal()
    {
        GOAPGoal bestGoal = null;
        float highestPriority = float.MinValue;

        foreach (var goal in goals)
        {
            // Only consider unsatisfied goals
            if (!PlannerIsSatisfied(worldState, goal))
            {
                // Select goal with highest priority
                if (goal.priority > highestPriority)
                {
                    bestGoal = goal;
                    highestPriority = goal.priority;
                }
            }
        }

        Debug.Log($"[{name}] Selected goal: {(bestGoal?.name ?? "None")}");
        return bestGoal;
    }

    /// <summary>
    /// Checks if a goal is satisfied by the current world state.
    /// </summary>
    /// <param name="state">The world state to check against.</param>
    /// <param name="goal">The goal to evaluate.</param>
    /// <returns>True if all desired state conditions are met.</returns>
    /// <remarks>
    /// Compares each desired state condition with the actual world state.
    /// Used for goal selection and plan validation.
    /// </remarks>
    protected bool PlannerIsSatisfied(WorldState state, GOAPGoal goal)
    {
        foreach (var kv in goal.desiredState)
        {
            if (!state.ContainsKey(kv.Key))
                return false;

            if (!state[kv.Key].Equals(kv.Value))
                return false;
        }

        return true;
    }

    /// <summary>
    /// Event handler for world state changes. Triggers reactive replanning.
    /// </summary>
    /// <param name="key">The world state key that changed.</param>
    /// <param name="value">The new value of the world state key.</param>
    /// <remarks>
    /// Only reacts to keys in the reactiveKeys set.
    /// Performs immediate cancellation and replanning when triggered.
    /// Critical for responsive AI behavior in dynamic environments.
    /// </remarks>
    protected virtual void OnWorldStateChanged(string key, object value)
    {
        // Ignore if not initialized
        if (!isInitialized)
            return;

        // Only react to subscribed keys
        if (!reactiveKeys.Contains(key))
            return;

        // Ignore if value didn't actually change
        var globalState = GlobalWorldState.Instance.State;
        if (globalState.ContainsKey(key) && globalState[key].Equals(value))
            return;

        Debug.Log($"[{name}] Reacting to world change: {key} = {value}");

        // 1. Stop all ongoing coroutines
        StopAllCoroutines();

        // 2. Cancel current action
        if (currentAction != null)
        {
            currentAction.Cancel();
            currentAction = null;
        }

        // 3. Clear action references
        currentActionRoutine = null;
        CurrentPlan = null;

        // 4. Force immediate replanning
        Debug.Log($"[{name}] Forcing reactive replanning");
        ChangeState(new ReplanningState(this));
    }
}
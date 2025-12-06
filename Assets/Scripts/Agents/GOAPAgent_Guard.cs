using UnityEngine;

/// <summary>
/// Guard AI agent specializing in patrol and law enforcement.
/// Demonstrates vigilance behavior with emergency response capabilities.
/// </summary>
/// <remarks>
/// Author: Miguel Angel Garcia Elizalde
/// Behavior Profile:
/// - Primary: Area patrol and surveillance
/// - Secondary: Threat apprehension
/// - Characteristics: Vigilant, reactive, protective
/// </remarks>
public class GOAPAgent_Guard : GOAPAgent
{
    /// <summary>
    /// Unity Start method override. Ensures proper initialization timing.
    /// </summary>
    private void Start()
    {
        StartCoroutine(DelayedInitialize());
    }

    /// <summary>
    /// Sets up the guard-specific world state values.
    /// </summary>
    /// <remarks>
    /// Defines patrol status and combat readiness.
    /// Maintains awareness of local and global threat levels.
    /// </remarks>
    protected override void PrepareWorldState()
    {
        // Local patrol state
        worldState["HasFinishedPatrolling"] = false;

        // Global awareness
        worldState["TownInDanger"] = false;
        worldState["ThiefCaught"] = false;

        // Local combat state
        worldState["GuardInRange"] = false;

        Debug.Log($"[{name}] Guard world state initialized");
    }

    /// <summary>
    /// Defines the guard's goals and their priorities.
    /// </summary>
    /// <remarks>
    /// Goal Structure:
    /// 1. Area patrol (normal duty)
    /// 2. Threat apprehension (emergency response)
    /// Equal priority for patrol and catch goals.
    /// </remarks>
    protected override void PrepareGoals()
    {
        // Primary goal: Area surveillance
        var patrollingGoal = new GOAPGoal("Patrol", 3.0f);
        patrollingGoal.desiredState["HasFinishedPatrolling"] = true;
        goals.Add(patrollingGoal);

        // Emergency goal: Law enforcement
        var catchGoal = new GOAPGoal("CatchThief", 3.0f);
        catchGoal.desiredState["ThiefCaught"] = true;
        goals.Add(catchGoal);

        // Reactive to town danger
        reactiveKeys.Add("TownInDanger");

        Debug.Log($"[{name}] Guard goals configured: Patrol, Catch");
    }
}
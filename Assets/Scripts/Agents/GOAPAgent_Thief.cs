using UnityEngine;

/// <summary>
/// Thief AI agent specializing in stealth, theft, and evasion.
/// Demonstrates self-preservation behavior with escape priorities.
/// </summary>
/// <remarks>
/// Author: Miguel Angel Garcia Elizalde
/// Behavior Profile:
/// - Primary: Theft and resource acquisition
/// - Secondary: Escape and survival
/// - Characteristics: Opportunistic, evasive, self-preserving
/// </remarks>
public class GOAPAgent_Thief : GOAPAgent
{
    /// <summary>
    /// Unity Start method override. Ensures proper initialization timing.
    /// </summary>
    private void Start()
    {
        StartCoroutine(DelayedInitialize());
    }

    /// <summary>
    /// Sets up the thief-specific world state values.
    /// </summary>
    /// <remarks>
    /// Defines theft progression and survival status.
    /// Monitors global threat levels without affecting them.
    /// </remarks>
    protected override void PrepareWorldState()
    {
        // Local theft progression
        worldState["IsAtItemsLocation"] = false;
        worldState["HasStolenItem"] = false;
        worldState["HasFled"] = false;
        worldState["IsAlive"] = true;

        // Global awareness (read-only for thief)
        worldState["TownInDanger"] = false;
        worldState["ThiefCaught"] = false;  // Monitors but doesn't set

        Debug.Log($"[{name}] Thief world state initialized");
    }

    /// <summary>
    /// Defines the thief's goals with strong survival priorities.
    /// </summary>
    /// <remarks>
    /// Goal Structure:
    /// 1. Theft execution (acquisition)
    /// 2. Escape (high priority survival)
    /// 3. Death handling (contingency)
    /// Survival goals have highest priority (10.0).
    /// </remarks>
    protected override void PrepareGoals()
    {
        // Primary goal: Resource acquisition
        var stealGoal = new GOAPGoal("Steal", 5.0f);
        stealGoal.desiredState["HasStolenItem"] = true;
        goals.Add(stealGoal);

        // High-priority goal: Escape and survival
        var fleeGoal = new GOAPGoal("Flee", 10.0f);
        fleeGoal.desiredState["HasFled"] = true;
        goals.Add(fleeGoal);

        // Contingency goal: Death handling
        var deathGoal = new GOAPGoal("Die", 10.0f);
        deathGoal.desiredState["IsAlive"] = false;
        goals.Add(deathGoal);

        // Reactive to danger and capture
        reactiveKeys.Add("TownInDanger");
        reactiveKeys.Add("ThiefCaught");

        Debug.Log($"[{name}] Thief goals configured: Steal, Flee, Die");
    }
}
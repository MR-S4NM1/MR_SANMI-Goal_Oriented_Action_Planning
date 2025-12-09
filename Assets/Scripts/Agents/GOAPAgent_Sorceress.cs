using UnityEngine;

/// <summary>
/// Sorceress AI agent specializing in resource gathering and magic-based responses.
/// Demonstrates multi-goal behavior with crafting and combat capabilities.
/// </summary>
/// <remarks>
/// Author: Miguel Angel Garcia Elizalde
/// Behavior Profile:
/// - Primary: Resource gathering and potion crafting
/// - Secondary: Emergency response to threats
/// - Characteristics: Support-oriented, reactive to danger
/// </remarks>
public class GOAPAgent_Sorceress : GOAPAgent
{
    /// <summary>
    /// Unity Start method override. Ensures proper initialization timing.
    /// </summary>
    private void Start()
    {
        StartCoroutine(DelayedInitialize());
    }

    /// <summary>
    /// Sets up the sorceress-specific world state values.
    /// </summary>
    /// <remarks>
    /// Defines both local states (crafting progress) and global awareness.
    /// Initial values represent starting conditions for behavior.
    /// </remarks>
    protected override void PrepareWorldState()
    {
        // Local states - crafting progression
        worldState["IsAtPlantsLocation"] = false;
        worldState["HasCollectedPlants"] = false;
        worldState["IsAtPotionsCraftLocation"] = false;
        worldState["HasPreparedPotions"] = false;

        // Global awareness
        worldState["TownInDanger"] = false;
        worldState["ThiefCaught"] = false;

        // Local combat state
        worldState["SorceressInRange"] = false;

        Debug.Log($"[{name}] Sorceress world state initialized");
    }

    /// <summary>
    /// Defines the sorceress's goals and their priorities.
    /// </summary>
    /// <remarks>
    /// Goal Structure:
    /// 1. Resource gathering (normal operations)
    /// 2. Potion crafting (production)
    /// 3. Threat response (emergency)
    /// All goals have equal priority for demonstration.
    /// </remarks>
    protected override void PrepareGoals()
    {
        // Primary goal: Resource collection
        var collectGoal = new GOAPGoal("CollectPlants", 1.0f);
        collectGoal.desiredState["HasCollectedPlants"] = true;
        goals.Add(collectGoal);

        // Secondary goal: Potion preparation
        var potionsGoal = new GOAPGoal("PreparePotions", 1.0f);
        potionsGoal.desiredState["HasPreparedPotions"] = true;
        goals.Add(potionsGoal);

        // Emergency goal: Threat neutralization
        var catchGoal = new GOAPGoal("CatchThief", 5.0f);
        catchGoal.desiredState["ThiefCaught"] = true;
        goals.Add(catchGoal);

        // Reactive to town danger
        reactiveKeys.Add("TownInDanger");

        Debug.Log($"[{name}] Sorceress goals configured: Collect, Craft, Catch");
    }
}

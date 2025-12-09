using UnityEngine;
using System.Collections;

/// <summary>
/// Guard combat action for apprehending and capturing the thief.
/// Final step in threat neutralization sequence with combat animation.
/// </summary>
/// <remarks>
/// Author: Miguel Angel Garcia Elizalde
/// Critical Action: Triggers global state change (ThiefCaught = true)
/// Animation: Combat attack with precise timing
/// Coordination: Works with AAGoAfterTheThief_Guard for full capture sequence
/// Global Impact: Changes ThiefCaught state affecting all agents
/// </remarks>
public class AApprehendThief_Guard : GOAPAction
{
    /// <summary>
    /// Unity Start method. Sets up apprehension preconditions and effects.
    /// </summary>
    private void Start()
    {
        // Ensure animation manager reference
        if (animationsManager == null)
            animationsManager = GetComponent<AnimationsManager>();

        // Preconditions: Must be in combat range during active danger
        AddPrecondition("TownInDanger", true);      // Emergency state
        AddPrecondition("GuardInRange", true);      // Combat proximity

        // Effect: Successfully captures thief (global state change)
        AddEffect("ThiefCaught", true);
        AddEffect("TownInDanger", false);
        AddEffect("HasFinishedPatrolling", false);
    }

    /// <summary>
    /// Executes the apprehension action with combat animation.
    /// </summary>
    /// <param name="state">Current world state.</param>
    /// <returns>IEnumerator for coroutine execution.</returns>
    /// <remarks>
    /// Execution Flow:
    /// 1. Get combat animation duration
    /// 2. Play attack animation
    /// 3. Wait for animation completion
    /// 4. Apply capture effect (triggers global reactions)
    /// </remarks>
    protected override IEnumerator PerformAction(WorldState state)
    {
        // Get precise animation timing for combat move
        currentAnimationDuration = animationsManager.GetAnimationLength("Melee Right Attack 01");

        // Execute combat animation
        animationsManager.AnimationFunction("Melee Right Attack 01", true);

        // Wait for attack animation to complete
        yield return new WaitForSeconds(currentAnimationDuration);

        // Apply capture effect (triggers ThiefCaught = true globally)
        Complete(state);

        Debug.Log($"[{name}] Thief apprehended successfully");
    }
}
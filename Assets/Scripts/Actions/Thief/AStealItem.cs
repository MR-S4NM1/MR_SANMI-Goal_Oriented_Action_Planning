using UnityEngine;
using System.Collections;

/// <summary>
/// Thief action for stealing items and triggering town alarm.
/// Demonstrates animation-synchronized action with world state impact.
/// </summary>
/// <remarks>
/// Author: Miguel Angel Garcia Elizalde
/// Critical Action: Triggers major world state change (TownInDanger)
/// Animation: Uses "Pick Up" animation with precise timing
/// Sequence: Part 2 of theft sequence (after AGoToSteal)
/// </remarks>
public class AStealItem : GOAPAction
{
    /// <summary>
    /// Unity Start method. Sets up stealing preconditions and effects.
    /// </summary>
    private void Start()
    {
        // Ensure animation manager reference
        if (animationsManager == null)
            animationsManager = GetComponent<AnimationsManager>();

        // Preconditions: Must be alive and at item location
        AddPrecondition("IsAlive", true);
        AddPrecondition("IsAtItemsLocation", true);

        // Effects: Item stolen and town alerted
        AddEffect("HasStolenItem", true);      // Personal achievement
        AddEffect("TownInDanger", true);       // Global alert trigger
    }

    /// <summary>
    /// Executes the stealing action with animation synchronization.
    /// </summary>
    /// <param name="state">Current world state.</param>
    /// <returns>IEnumerator for coroutine execution.</returns>
    /// <remarks>
    /// Execution Flow:
    /// 1. Get animation duration for precise timing
    /// 2. Start pick-up animation
    /// 3. Wait exact animation duration
    /// 4. Apply effects (triggers global reaction)
    /// </remarks>
    protected override IEnumerator PerformAction(WorldState state)
    {
        // Get precise animation duration for timing
        currentAnimationDuration = animationsManager.GetAnimationLength("Pick Up");

        // Start stealing animation
        animationsManager.AnimationFunction("Pick Up", true);

        // Timer for animation completion
        float timer = 0f;

        // Wait for animation to complete
        while (timer < currentAnimationDuration)
        {
            // Check for cancellation
            if (isCancelled)
            {
                Debug.LogWarning($"[GOAPAction] {name} cancelled mid-action!");
                yield break;
            }

            // Update timer
            timer += Time.deltaTime;
            yield return null;
        }

        // Complete action (triggers TownInDanger = true globally)
        Complete(state);
    }
}
using UnityEngine;
using System.Collections;

/// <summary>
/// Sorceress action for brewing magical potions with full state reset.
/// Final step in crafting sequence with comprehensive state cleanup.
/// </summary>
/// <remarks>
/// Author: Miguel Angel Garcia Elizalde
/// Crafting Sequence: Step 4 of 4 (completion)
/// State Management: Resets all crafting states for new cycle
/// Animation: Uses "Drink Potion" animation (brewing representation)
/// Cycle Design: Enables continuous crafting loops
/// </remarks>
public class APreparePotions : GOAPAction
{
    /// <summary>
    /// Unity Start method. Sets up potion crafting preconditions and effects.
    /// </summary>
    private void Start()
    {
        // Ensure animation manager reference
        if (animationsManager == null)
            animationsManager = GetComponent<AnimationsManager>();

        // Preconditions: Must be at station and haven't crafted yet
        AddPrecondition("IsAtPotionsCraftLocation", true);
        AddPrecondition("HasPreparedPotions", false);

        // Primary effect: Potions successfully crafted
        AddEffect("HasPreparedPotions", true);

        // State reset effects: Clear all intermediate states for new cycle
        AddEffect("IsAtPlantsLocation", false);           // Reset location
        AddEffect("HasCollectedPlants", false);           // Clear inventory
        AddEffect("IsAtPotionsCraftLocation", false);     // Leave station
    }

    /// <summary>
    /// Executes potion brewing with animation synchronization.
    /// </summary>
    /// <param name="state">Current world state.</param>
    /// <returns>IEnumerator for coroutine execution.</returns>
    /// <remarks>
    /// Simple timed animation suitable for brewing actions.
    /// Applies multiple state effects upon completion.
    /// </remarks>
    protected override IEnumerator PerformAction(WorldState state)
    {
        // Get brewing animation duration
        currentAnimationDuration = animationsManager.GetAnimationLength("Drink Potion");

        // Trigger brewing animation
        animationsManager.AnimationFunction("Drink Potion", true);

        // Wait for brewing to complete
        yield return new WaitForSeconds(currentAnimationDuration);

        // Complete crafting (applies multiple state changes)
        Complete(state);
    }
}
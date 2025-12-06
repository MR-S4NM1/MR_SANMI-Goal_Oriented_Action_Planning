using UnityEngine;
using System.Collections;

/// <summary>
/// Sorceress action for collecting magical herbs with animation synchronization.
/// Demonstrates timed resource gathering with precise animation matching.
/// </summary>
/// <remarks>
/// Author: Miguel Angel Garcia Elizalde
/// Crafting Sequence: Step 2 of 4
/// Resource Management: Transitions from location presence to resource possession
/// Animation: Uses "Pick Up" animation matching timing exactly
/// Prerequisite: Must be at plant location with empty inventory
/// </remarks>
public class ACollectPlants : GOAPAction
{
    /// <summary>
    /// Unity Start method. Sets up plant collection preconditions and effects.
    /// </summary>
    private void Start()
    {
        // Ensure animation manager reference
        if (animationsManager == null)
            animationsManager = GetComponent<AnimationsManager>();

        // Preconditions: Must be at location and not already collected
        AddPrecondition("IsAtPlantsLocation", true);
        AddPrecondition("HasCollectedPlants", false);

        // Effect: Successfully gathered resources
        AddEffect("HasCollectedPlants", true);
    }

    /// <summary>
    /// Executes plant collection with precise animation timing.
    /// </summary>
    /// <param name="state">Current world state.</param>
    /// <returns>IEnumerator for coroutine execution.</returns>
    /// <remarks>
    /// Uses exact animation duration for realistic gathering timing.
    /// Simple yield pattern suitable for non-interruptible animations.
    /// </remarks>
    protected override IEnumerator PerformAction(WorldState state)
    {
        // Get precise animation duration
        currentAnimationDuration = animationsManager.GetAnimationLength("Pick Up");

        // Trigger gathering animation
        animationsManager.AnimationFunction("Pick Up", true);

        // Wait exact animation duration
        yield return new WaitForSeconds(currentAnimationDuration);

        // Complete resource collection
        Complete(state);
    }
}
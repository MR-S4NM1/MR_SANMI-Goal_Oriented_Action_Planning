using UnityEngine;
using System.Collections;

/// <summary>
/// Thief death action triggered by capture.
/// Final action in thief behavior tree with scene cleanup.
/// </summary>
/// <remarks>
/// Author: Miguel Angel Garcia Elizalde
/// Terminal Action: Ends thief's participation in simulation
/// World Impact: Clears TownInDanger status
/// Cleanup: Deactivates GameObject after animation
/// Sequence: Triggered by ThiefCaught = true global state
/// </remarks>
public class ACaught : GOAPAction
{
    /// <summary>
    /// Unity Start method. Sets up death preconditions and effects.
    /// </summary>
    private void Start()
    {
        // Ensure animation manager reference
        if (animationsManager == null)
            animationsManager = GetComponent<AnimationsManager>();

        // Preconditions: Must be alive and captured
        AddPrecondition("IsAlive", true);
        AddPrecondition("ThiefCaught", true);

        // Effects: Death and danger clearance
        AddEffect("IsAlive", false);          // Personal death
        AddEffect("TownInDanger", false);     // Clears global alert
    }

    /// <summary>
    /// Executes death animation and cleans up thief GameObject.
    /// </summary>
    /// <param name="state">Current world state.</param>
    /// <returns>IEnumerator for coroutine execution.</returns>
    /// <remarks>
    /// Execution Flow:
    /// 1. Play death animation
    /// 2. Apply death effects to world state
    /// 3. Deactivate GameObject (scene cleanup)
    /// </remarks>
    protected override IEnumerator PerformAction(WorldState state)
    {
        Debug.Log("Ladrón muriendo...");

        // Get death animation duration
        currentAnimationDuration = animationsManager.GetAnimationLength("Die");

        // Trigger death animation
        animationsManager.AnimationFunction("Die", true);

        // Wait for animation (slightly shortened for smooth transition)
        yield return new WaitForSeconds(currentAnimationDuration - 0.5f);

        // Apply death effects to world state
        Complete(state);

        // Remove thief from scene
        gameObject.SetActive(false);
    }
}
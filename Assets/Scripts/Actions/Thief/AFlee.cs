using UnityEngine;
using System.Collections;

/// <summary>
/// Thief escape action with emergency death contingency.
/// Demonstrates complex action with conditional execution paths.
/// </summary>
/// <remarks>
/// Author: Miguel Angel Garcia Elizalde
/// Multi-Path Action:
/// - Primary: Escape to safety
/// - Contingency: Death if captured during escape
/// World Impact: Clears TownInDanger upon successful escape
/// Self-Destruction: Deactivates GameObject on completion
/// </remarks>
public class AFlee : AGoToPosition
{
    /// <summary>
    /// Unity Start method. Sets up escape preconditions and effects.
    /// </summary>
    private void Start()
    {
        // Ensure animation manager reference
        if (animationsManager == null)
            animationsManager = GetComponent<AnimationsManager>();

        // Preconditions: Must be alive, in danger, has stolen item, hasn't fled
        AddPrecondition("IsAlive", true);
        AddPrecondition("TownInDanger", true);
        AddPrecondition("HasStolenItem", true);
        AddPrecondition("HasFled", false);

        // Effects: Escape successful and danger cleared
        AddEffect("HasFled", true);           // Personal escape status
        AddEffect("TownInDanger", false);     // Clears global alert
    }

    /// <summary>
    /// Executes escape action with capture contingency.
    /// </summary>
    /// <param name="state">Current world state.</param>
    /// <returns>IEnumerator for coroutine execution.</returns>
    /// <remarks>
    /// Execution Paths:
    /// 1. Normal: Move to escape point -> Complete -> Deactivate
    /// 2. Captured: Die mid-escape -> Complete -> Deactivate
    /// </remarks>
    protected override IEnumerator PerformAction(WorldState state)
    {
        // Start escape movement
        animationsManager.AnimationFunction("Walk", true);

        // Move towards escape point
        while (Vector3.Distance(transform.position, target.position) > distanceThreshold)
        {
            // Check for capture during escape
            if (isCancelled || (bool)state["ThiefCaught"] == true)
            {
                // Contingency: Captured during escape -> Die
                currentAnimationDuration = animationsManager.GetAnimationLength("Die");
                animationsManager.AnimationFunction("Die", true);

                // Wait for death animation (slightly shortened)
                yield return new WaitForSeconds(currentAnimationDuration - 0.5f);

                // Complete with death effects
                Complete(state);

                // Deactivate thief (removed from scene)
                gameObject.SetActive(false);
                yield break;
            }

            // Continue movement
            transform.position = Vector3.MoveTowards(
                transform.position,
                target.position,
                Time.deltaTime * movementSpeed);

            // Face movement direction
            transform.LookAt(new Vector3(
                target.position.x,
                transform.position.y,
                target.position.z));

            yield return null;
        }

        // Brief pause at destination
        yield return new WaitForSeconds(0.2f);

        // Complete successful escape
        Complete(state);

        // Deactivate escaped thief
        gameObject.SetActive(false);
    }
}
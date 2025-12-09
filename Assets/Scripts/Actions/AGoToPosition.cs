using UnityEngine;
using System.Collections;

/// <summary>
/// Base movement action for navigating to a target position.
/// Provides reusable movement logic with animation synchronization.
/// </summary>
/// <remarks>
/// Author: Miguel Angel Garcia Elizalde
/// Inheritance: Base class for all movement-based actions
/// Features: Path following, rotation control, distance threshold
/// Extensibility: Override Start() for custom preconditions/effects
/// </remarks>

public class AGoToPosition : GOAPAction
{
    /// <summary>
    /// Target position to move towards.
    /// </summary>
    [SerializeField]
    [Tooltip("Target transform to move towards.")]
    protected Transform target;

    /// <summary>
    /// Distance threshold for considering the target reached.
    /// </summary>
    [SerializeField]
    [Tooltip("Distance at which target is considered reached.")]
    protected float distanceThreshold = 1.5f;

    /// <summary>
    /// Movement speed in units per second.
    /// </summary>
    [SerializeField]
    [Tooltip("Movement speed in units per second.")]
    protected float movementSpeed = 1.0f;

    /// <summary>
    /// Unity Awake method override. Ensures animation manager is available.
    /// </summary>
    private void Awake()
    {
        if (animationsManager == null)
            animationsManager = GetComponent<AnimationsManager>();
    }

    /// <summary>
    /// Executes the movement action. Moves towards target until within threshold.
    /// </summary>
    /// <param name="state">Current world state (unused in base implementation).</param>
    /// <returns>IEnumerator for coroutine execution.</returns>
    /// <remarks>
    /// Execution Flow:
    /// 1. Start walk animation
    /// 2. Move towards target each frame
    /// 3. Face movement direction
    /// 4. Check for cancellation
    /// 5. Complete when within threshold
    /// </remarks>
    protected override IEnumerator PerformAction(WorldState state)
    {
        // Start movement animation
        animationsManager.AnimationFunction("Walk", true);

        // Move until within threshold distance
        while (Vector3.Distance(transform.position, target.position) > distanceThreshold)
        {
            // Calculate movement for this frame
            transform.position = Vector3.MoveTowards(
                transform.position,
                target.position,
                Time.deltaTime * movementSpeed);

            // Rotate to face movement direction
            transform.LookAt(new Vector3(
                target.position.x,
                transform.position.y,
                target.position.z));

            yield return null;
        }

        // Brief pause for animation smoothing
        yield return new WaitForSeconds(0.2f);

        // Mark action as complete
        Complete(state);
    }
}
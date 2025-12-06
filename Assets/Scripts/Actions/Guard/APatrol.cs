using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Guard patrol action for systematic area surveillance with waypoint-based routing.
/// Demonstrates continuous behavior with interruption handling for emergency response.
/// </summary>
/// <remarks>
/// Author: Miguel Angel Garcia Elizalde
/// Behavior Pattern: Loop-based patrol with dwell times
/// Interruption: Automatically cancels when TownInDanger = true
/// Optimization: Starts from nearest waypoint, circular route
/// State Management: Maintains patrol progress through interruptions
/// </remarks>
public class APatrol : GOAPAction
{
    /// <summary>
    /// List of patrol waypoints defining the patrol route.
    /// </summary>
    [SerializeField]
    [Tooltip("List of waypoints defining the patrol route. Order determines patrol sequence.")]
    protected List<Transform> targets = new List<Transform>();

    /// <summary>
    /// Distance threshold for considering a waypoint reached.
    /// </summary>
    [SerializeField]
    [Tooltip("Distance at which waypoint is considered reached.")]
    protected float distanceThreshold = 1.5f;

    /// <summary>
    /// Movement speed during patrol in units per second.
    /// </summary>
    [SerializeField]
    [Tooltip("Movement speed during patrol in units per second.")]
    protected float movementSpeed = 1.0f;

    /// <summary>
    /// Time to wait at each waypoint before proceeding to next.
    /// </summary>
    [SerializeField]
    [Tooltip("Time to wait at each waypoint (seconds) before continuing patrol.")]
    protected float waitTimeAtWaypoint = 3.0f;

    /// <summary>
    /// Current target waypoint index in the patrol route.
    /// </summary>
    protected int currentIndex = 0;

    /// <summary>
    /// Flag indicating if currently waiting at a waypoint.
    /// </summary>
    protected bool isWaiting = false;

    /// <summary>
    /// Unity Start method. Sets up patrol-specific preconditions and effects.
    /// </summary>
    private void Start()
    {
        // Ensure animation manager reference
        if (animationsManager == null)
            animationsManager = GetComponent<AnimationsManager>();

        // Precondition: Only patrol when town is safe
        AddPrecondition("TownInDanger", false);

        // Effect: Mark patrol as completed (though patrol is continuous)
        AddEffect("HasFinishedPatrolling", true);
    }

    /// <summary>
    /// Executes continuous patrol behavior with waypoint navigation.
    /// </summary>
    /// <param name="state">Current world state for interruption checks.</param>
    /// <returns>IEnumerator for coroutine execution.</returns>
    /// <remarks>
    /// Execution Flow:
    /// 1. Find nearest starting waypoint
    /// 2. Enter patrol loop (move -> wait -> next waypoint)
    /// 3. Continuously check for danger interruptions
    /// 4. Exit loop when cancelled or danger detected
    /// </remarks>
    protected override IEnumerator PerformAction(WorldState state)
    {
        Debug.Log($"[APatrol] Starting patrol with {targets.Count} waypoints");

        // 1. Find optimal starting point (nearest waypoint)
        currentIndex = FindNearestWaypointIndex();

        // Start patrol movement animation
        animationsManager.AnimationFunction("Walk", true);

        // 2. Continuous patrol loop
        while (!isCancelled)
        {
            // Validate patrol route
            if (targets.Count == 0)
            {
                Debug.LogError("[APatrol] No waypoints assigned!");
                yield break;
            }

            // Emergency interruption check
            if (isCancelled || (bool)state["TownInDanger"] == true)
            {
                Debug.Log("[APatrol] Patrol interrupted by danger or cancellation");
                Complete(state);
                yield break;
            }

            Transform currentTarget = targets[currentIndex];
            float distance = Vector3.Distance(transform.position, currentTarget.position);

            // 3. Move towards current waypoint
            if (distance > distanceThreshold)
            {
                // Movement towards target
                transform.position = Vector3.MoveTowards(
                    transform.position,
                    currentTarget.position,
                    Time.deltaTime * movementSpeed);

                // Face movement direction
                transform.LookAt(new Vector3(
                    currentTarget.position.x,
                    transform.position.y,
                    currentTarget.position.z));

                yield return null;
                continue;
            }

            // 4. Arrived at waypoint - wait period
            if (!isWaiting)
            {
                Debug.Log($"[APatrol] Reached waypoint {currentIndex}. Waiting {waitTimeAtWaypoint} seconds...");

                // Switch to idle animation during wait
                animationsManager.AnimationFunction("Idle", true);
                isWaiting = true;

                // Wait at waypoint
                yield return new WaitForSeconds(waitTimeAtWaypoint);

                // 5. Move to next waypoint (circular route)
                currentIndex = (currentIndex + 1) % targets.Count;
                Debug.Log($"[APatrol] Next waypoint: {currentIndex}");

                // Resume movement animation
                animationsManager.AnimationFunction("Walk", true);
                isWaiting = false;
            }

            yield return null;
        }

        // This point is only reached if patrol completes normally (rare)
        Debug.Log("[APatrol] Patrol completed normally");
        Complete(state);
    }

    /// <summary>
    /// Finds the nearest waypoint index to start patrol efficiently.
    /// </summary>
    /// <returns>Index of nearest waypoint in targets list.</returns>
    /// <remarks>
    /// Optimization: Reduces initial travel time by starting at closest point
    /// Error Handling: Returns 0 if no valid waypoints
    /// </remarks>
    protected int FindNearestWaypointIndex()
    {
        if (targets.Count == 0)
        {
            Debug.LogWarning("[APatrol] No waypoints to find nearest");
            return 0;
        }

        int nearestIndex = 0;
        float nearestDistance = float.MaxValue;

        // Find closest waypoint
        for (int i = 0; i < targets.Count; i++)
        {
            if (targets[i] == null)
            {
                Debug.LogWarning($"[APatrol] Waypoint {i} is null");
                continue;
            }

            float distance = Vector3.Distance(transform.position, targets[i].position);
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestIndex = i;
            }
        }

        Debug.Log($"[APatrol] Nearest waypoint: {nearestIndex} (distance: {nearestDistance:F2})");
        return nearestIndex;
    }

    /// <summary>
    /// Custom cancellation handler for patrol action.
    /// Ensures clean transition from patrol to other behaviors.
    /// </summary>
    /// <remarks>
    /// Stops waiting state and returns to idle animation.
    /// Called when action is interrupted by planner or reactive event.
    /// </remarks>
    public override void Cancel()
    {
        // Call base cancellation logic
        base.Cancel();

        // Clear waiting state
        isWaiting = false;

        // Return to neutral animation state
        animationsManager.AnimationFunction("Idle", true);

        Debug.Log($"[APatrol] Patrol cancelled at waypoint {currentIndex}");
    }
}
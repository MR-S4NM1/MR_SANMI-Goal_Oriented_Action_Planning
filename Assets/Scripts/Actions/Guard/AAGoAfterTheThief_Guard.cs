/// <summary>
/// Guard movement action for pursuing the thief during emergency response.
/// Specialized movement to close distance with target for apprehension.
/// </summary>
/// <remarks>
/// Author: Miguel Angel Garcia Elizalde
/// Emergency Response: Part of threat neutralization sequence
/// Prerequisites: Active danger, thief not yet caught
/// Purpose: Position guard within combat range
/// Sequence: Step 1 of capture (movement)
/// </remarks>
public class AAGoAfterTheThief_Guard : AGoToPosition
{
    /// <summary>
    /// Unity Start method. Sets up pursuit-specific preconditions and effects.
    /// </summary>
    private void Start()
    {
        // Ensure animation manager reference
        if (animationsManager == null)
            animationsManager = GetComponent<AnimationsManager>();

        // Preconditions: Only pursue during active danger when thief is free
        AddPrecondition("TownInDanger", true);      // Emergency condition
        AddPrecondition("ThiefCaught", false);      // Target not captured

        // Effect: Guard reaches combat engagement range
        AddEffect("GuardInRange", true);
    }

    // Note: Movement logic inherited from AGoToPosition
    // Uses walk animation and standard movement mechanics
}
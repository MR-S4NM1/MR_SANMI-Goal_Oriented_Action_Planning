/// <summary>
/// Thief movement action for navigating to stealable item locations.
/// Specialized version of AGoToPosition with theft-specific preconditions.
/// </summary>
/// <remarks>
/// Author: Miguel Angel Garcia Elizalde
/// Behavior Context: Part of theft sequence
/// Preconditions: Must be alive, haven't stolen, no danger, not at location
/// Effects: Arrives at item location
/// </remarks>
public class AGoToSteal : AGoToPosition
{
    /// <summary>
    /// Unity Start method. Sets up theft-specific preconditions and effects.
    /// </summary>
    private void Start()
    {
        // Ensure animation manager reference
        if (animationsManager == null)
            animationsManager = GetComponent<AnimationsManager>();

        // Preconditions: Must be in valid state to initiate theft
        AddPrecondition("IsAlive", true);           // Must be alive
        AddPrecondition("HasStolenItem", false);    // Haven't stolen yet
        AddPrecondition("TownInDanger", false);     // No active danger
        AddPrecondition("IsAtItemsLocation", false); // Not already at location

        // Effect: Arrives at theft location
        AddEffect("IsAtItemsLocation", true);
    }
}
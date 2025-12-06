/// <summary>
/// Sorceress movement action for navigating to potion crafting station.
/// Transitions from resource gathering to crafting phase.
/// </summary>
/// <remarks>
/// Author: Miguel Angel Garcia Elizalde
/// Crafting Sequence: Step 3 of 4
/// Transition: Resource holder -> Crafter position
/// Prerequisite: Must have collected plants first
/// Purpose: Positions at crafting station with resources
/// </remarks>
public class AGoToPreparePotions : AGoToPosition
{
    /// <summary>
    /// Unity Start method. Sets up crafting movement preconditions and effects.
    /// </summary>
    private void Start()
    {
        // Ensure animation manager reference
        if (animationsManager == null)
            animationsManager = GetComponent<AnimationsManager>();

        // Preconditions: Must have resources and not already at station
        AddPrecondition("HasCollectedPlants", true);
        AddPrecondition("IsAtPotionsCraftLocation", false);

        // Effect: Arrives at crafting location
        AddEffect("IsAtPotionsCraftLocation", true);
    }
}
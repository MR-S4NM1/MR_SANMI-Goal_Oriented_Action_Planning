/// <summary>
/// Sorceress movement action for navigating to magical herb gathering locations.
/// First step in potion crafting sequence with state reset capability.
/// </summary>
/// <remarks>
/// Author: Miguel Angel Garcia Elizalde
/// Crafting Sequence: Step 1 of 4 (Movement -> Collection -> Movement -> Crafting)
/// State Management: Resets crafting completion flag for new cycles
/// Purpose: Positions sorceress at resource location for gathering
/// </remarks>
public class AGetPlants : AGoToPosition
{
    /// <summary>
    /// Unity Start method. Sets up plant gathering movement preconditions and effects.
    /// </summary>
    private void Start()
    {
        // Ensure animation manager reference
        if (animationsManager == null)
            animationsManager = GetComponent<AnimationsManager>();

        // Precondition: Not already at plant location
        AddPrecondition("IsAtPlantsLocation", false);

        // Primary effect: Arrives at gathering location
        AddEffect("IsAtPlantsLocation", true);

        // Secondary effect: Resets crafting state for new cycle
        AddEffect("HasPreparedPotions", false);
    }
}
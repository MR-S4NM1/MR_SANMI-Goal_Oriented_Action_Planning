using UnityEngine;

/// <summary>
/// Sorceress emergency movement action for pursuing threats with magical approach.
/// Emergency response equivalent to guard pursuit with magical specialization.
/// </summary>
/// <remarks>
/// Author: Miguel Angel Garcia Elizalde
/// Emergency Response: Part 1 of 2 (Movement -> Magic)
/// Specialization: Magical pursuit (differentiated from guard's physical pursuit)
/// Prerequisite: Active town danger
/// Purpose: Positions for magical intervention
/// </remarks>
public class AAGoAfterTheThief_Sorceress : AGoToPosition
{
    /// <summary>
    /// Unity Start method. Sets up magical pursuit preconditions and effects.
    /// </summary>
    private void Start()
    {
        // Ensure animation manager reference
        if (animationsManager == null)
            animationsManager = GetComponent<AnimationsManager>();

        // Precondition: Only pursue during active danger
        AddPrecondition("TownInDanger", true);

        // Effect: Reaches magical combat range
        AddEffect("SorceressInRange", true);
    }
}
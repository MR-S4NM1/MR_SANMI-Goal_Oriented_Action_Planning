using UnityEngine;
using System.Collections;

/// <summary>
/// Sorceress magical combat action for apprehending threats with spellcasting.
/// Demonstrates interruptible animation with frame-by-frame cancellation checking.
/// </summary>
/// <remarks>
/// Author: Miguel Angel Garcia Elizalde
/// Emergency Response: Part 2 of 2 (final capture)
/// Magic Specialization: Spellcasting vs physical combat
/// Animation: Interruptible with frame precision
/// Global Impact: Same as guard (ThiefCaught = true) via different method
/// </remarks>
public class AAprehend_Sorceress : GOAPAction
{
    /// <summary>
    /// Unity Start method. Sets up magical apprehension preconditions and effects.
    /// </summary>
    private void Start()
    {
        // Ensure animation manager reference
        if (animationsManager == null)
            animationsManager = GetComponent<AnimationsManager>();

        // Preconditions: Active danger and within magical range
        AddPrecondition("TownInDanger", true);
        AddPrecondition("SorceressInRange", true);

        // Effect: Successfully captures thief via magic
        AddEffect("ThiefCaught", true);
    }

    /// <summary>
    /// Executes magical apprehension with frame-accurate cancellation checking.
    /// </summary>
    /// <param name="state">Current world state.</param>
    /// <returns>IEnumerator for coroutine execution.</returns>
    /// <remarks>
    /// Uses frame-by-frame timing for precise interruption handling.
    /// More responsive than simple WaitForSeconds for combat actions.
    /// </remarks>
    protected override IEnumerator PerformAction(WorldState state)
    {
        // Get spellcasting animation duration
        currentAnimationDuration = animationsManager.GetAnimationLength("Cast Spell");

        // Begin spellcasting animation
        animationsManager.AnimationFunction("Cast Spell", true);

        // Frame-accurate timing with cancellation checks
        float timer = 0f;
        while (timer < currentAnimationDuration)
        {
            // Check for cancellation every frame
            if (isCancelled)
            {
                Debug.LogWarning($"[GOAPAction] {name} cancelled mid-action!");
                yield break;
            }

            // Increment timer
            timer += Time.deltaTime;
            yield return null;
        }

        // Complete magical capture
        Complete(state);
    }
}
using UnityEngine;

/// <summary>
/// Centralized animation controller for GOAP agents that manages Unity Animator state.
/// Provides abstraction layer between AI logic and animation system with safety features.
/// </summary>
/// <remarks>
/// Author: Miguel Angel Garcia Elizalde
/// Pattern: Facade for Unity Animator
/// Features: Parameter cleanup, animation length lookup, error handling
/// Safety: Null checks, parameter validation, warning messages
/// </remarks>

[RequireComponent(typeof(Animator))]
[DisallowMultipleComponent]
public class AnimationsManager : MonoBehaviour
{
    /// <summary>
    /// Reference to the Unity Animator component.
    /// Serialized for manual assignment in the Inspector.
    /// </summary>
    [Tooltip("Reference to the Animator component. Auto-assigned if null.")]
    [SerializeField] protected Animator animator;

    /// <summary>
    /// Ensures the Animator reference is valid when the component is enabled.
    /// </summary>
    /// <remarks>
    /// Called by Unity when the component becomes active.
    /// Provides fallback assignment if the serialized field is empty.
    /// </remarks>
    private void OnEnable()
    {
        if (animator == null)
            animator = GetComponent<Animator>();
    }

    /// <summary>
    /// Primary method for triggering animations via boolean or trigger parameters.
    /// </summary>
    /// <param name="animationName">Name of the animation parameter in the Animator.</param>
    /// <param name="useTrigger">
    /// True: Uses SetTrigger (one-shot animations)
    /// False: Uses SetBool (state-based animations)
    /// </param>
    /// <remarks>
    /// Automatically clears all boolean parameters before setting new ones
    /// to prevent animation state conflicts.
    /// </remarks>
    public void AnimationFunction(string animationName, bool useTrigger)
    {
        // Ensure clean animation state transition
        ClearAnimationParameters();

        if (!useTrigger)
            animator.SetBool(animationName, true);   // State-based animation
        else
            animator.SetTrigger(animationName);      // One-shot animation
    }

    /// <summary>
    /// Resets all boolean parameters in the Animator to false.
    /// Prevents conflicting animation states when transitioning between behaviors.
    /// </summary>
    /// <remarks>
    /// Critical for FSM-based animators to ensure clean state transitions.
    /// Only affects boolean parameters; triggers and floats remain unchanged.
    /// </remarks>
    public void ClearAnimationParameters()
    {
        foreach (AnimatorControllerParameter param in animator.parameters)
        {
            if (param.type == AnimatorControllerParameterType.Bool)
            {
                animator.SetBool(param.name, false);
            }
        }
    }

    /// <summary>
    /// Retrieves the duration of a specific animation clip.
    /// </summary>
    /// <param name="animationName">Name of the animation clip to query.</param>
    /// <returns>
    /// Length of the animation in seconds, or 0f if not found.
    /// </returns>
    /// <remarks>
    /// Useful for synchronizing action durations with animation lengths.
    /// Searches through all clips in the runtime animator controller.
    /// </remarks>
    public float GetAnimationLength(string animationName)
    {
        foreach (var clip in animator.runtimeAnimatorController.animationClips)
        {
            if (clip.name == animationName)
                return clip.length;
        }

        Debug.LogWarning($"Clip '{animationName}' no encontrado.");
        return 0f;
    }
}

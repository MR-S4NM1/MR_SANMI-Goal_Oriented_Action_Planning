using UnityEngine;
using System.Collections;

public class AnimationsManager : MonoBehaviour
{
    [SerializeField] protected Animator animator;

    private void OnEnable()
    {
        if (animator == null)
            animator = GetComponent<Animator>();
    }

    public void AnimationFunction(string p_animationName, bool trigger)
    {
        ClearAnimationParameters();

        if (!trigger)
            animator.SetBool(p_animationName, true);
        else
            animator.SetTrigger(p_animationName);
    }

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

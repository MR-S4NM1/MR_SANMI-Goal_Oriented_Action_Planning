using UnityEngine;
using System.Collections;

public class AAprehend_Sorceress : GOAPAction 
{
    private void Start()
    {
        if (animationsManager == null)
            animationsManager = GetComponent<AnimationsManager>();

        AddPrecondition("TownInDanger", true);
        AddPrecondition("SorceressInRange", true);
        AddEffect("ThiefCaught", true);
    }


    protected override IEnumerator PerformAction(WorldState state)
    {
        currentAnimationDuration = animationsManager.GetAnimationLength("Cast Spell");
        animationsManager.AnimationFunction("Cast Spell", true);

        float timer = 0f;

        while (timer < currentAnimationDuration)
        {
            if (isCancelled)
            {
                Debug.LogWarning($"[GOAPAction] {name} cancelled mid-action!");
                yield break;
            }

            timer += Time.deltaTime;
            yield return null;
        }

        Complete(state);
    }
}
using UnityEngine;
using System.Collections;

public class AStealItem : GOAPAction
{
    private void Start()
    {
        if (animationsManager == null)
            animationsManager = GetComponent<AnimationsManager>();

        AddPrecondition("IsAlive", true);
        AddPrecondition("IsAtItemsLocation", true);
        AddEffect("HasStolenItem", true);
        AddEffect("TownInDanger", true);
    }

    protected override IEnumerator PerformAction(WorldState state)
    {
        currentAnimationDuration = animationsManager.GetAnimationLength("Pick Up");
        animationsManager.AnimationFunction("Pick Up", true);

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

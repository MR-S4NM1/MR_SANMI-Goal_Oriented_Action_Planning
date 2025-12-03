using UnityEngine;
using System.Collections;

public class ACollectPlants : GOAPAction
{
    private void Awake()
    {
        if (animationsManager == null)
            animationsManager = GetComponent<AnimationsManager>();

        AddPrecondition("IsAtPlantsLocation", true);
        AddEffect("HasCollectedPlants", true);
    }

    protected override IEnumerator PerformAction(WorldState state)
    {
        currentAnimationDuration = animationsManager.GetAnimationLength("Pick Up");
        animationsManager.AnimationFunction("Pick Up", true);

        yield return new WaitForSeconds(currentAnimationDuration + 0.5f);
        Complete(state);
    }
}

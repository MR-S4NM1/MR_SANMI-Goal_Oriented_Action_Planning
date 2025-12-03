using UnityEngine;
using System.Collections;

public class APreparePotions : GOAPAction
{
    private void Awake()
    {
        if (animationsManager == null)
            animationsManager = GetComponent<AnimationsManager>();

        AddPrecondition("IsAtPotionsCraftLocation", true);
        AddEffect("HasPreparedPotions", true);

        AddEffect("IsAtPlantsLocation", false);
        AddEffect("HasCollectedPlants", false);
        AddEffect("IsAtPotionsCraftLocation", false);

        AddEffect("TownInDanger", false);
    }

    protected override IEnumerator PerformAction(WorldState state)
    {
        currentAnimationDuration = animationsManager.GetAnimationLength("Drink Potion");
        animationsManager.AnimationFunction("Drink Potion", true);

        yield return new WaitForSeconds(currentAnimationDuration);
        Complete(state);
    }
}

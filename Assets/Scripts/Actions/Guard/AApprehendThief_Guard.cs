using UnityEngine;
using System.Collections;

public class AApprehendThief_Guard : GOAPAction
{
    private void Start()
    {
        if (animationsManager == null)
            animationsManager = GetComponent<AnimationsManager>();

        AddPrecondition("TownInDanger", true);
        AddPrecondition("GuardInRange", true);
        AddEffect("ThiefCaught", true);
    }

    protected override IEnumerator PerformAction(WorldState state)
    {
        currentAnimationDuration = animationsManager.GetAnimationLength("Melee Right Attack 01"); 
        animationsManager.AnimationFunction("Melee Right Attack 01", true);

        yield return new WaitForSeconds(currentAnimationDuration);

        Complete(state);
    }
}

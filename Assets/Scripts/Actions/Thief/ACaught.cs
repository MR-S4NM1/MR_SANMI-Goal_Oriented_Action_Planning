using UnityEngine;
using System.Collections;

public class ACaught : GOAPAction
{
    private void Start()
    {
        if (animationsManager == null)
            animationsManager = GetComponent<AnimationsManager>();

        AddPrecondition("IsAlive", true);
        AddPrecondition("ThiefCaught", true);

        AddEffect("IsAlive", false);

        AddEffect("TownInDanger", false);
    }

    protected override IEnumerator PerformAction(WorldState state)
    {
        Debug.Log("Ladrón muriendo...");

        currentAnimationDuration = animationsManager.GetAnimationLength("Die");
        animationsManager.AnimationFunction("Die", true);

        yield return new WaitForSeconds(currentAnimationDuration - 0.5f);

        Complete(state);
        gameObject.SetActive(false);

    }
}
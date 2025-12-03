using System.Collections;
using UnityEngine;

public class ASleep : GOAPAction
{
    private void OnEnable()
    {
        if (animationsManager == null)
            animationsManager = GetComponent<AnimationsManager>();

        AddPrecondition("IsTired", true);
        AddEffect("IsWellRest", true);
        AddEffect("IsTired", false);
    }

    protected override IEnumerator PerformAction(WorldState state)
    {
        Debug.Log("Sleeping...");
        yield return new WaitForSeconds(2f);

        Complete(state);
    }
}

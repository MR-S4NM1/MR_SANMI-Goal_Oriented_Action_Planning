using System.Collections;
using UnityEngine;

public class AEat : GOAPAction
{
    private void Awake()
    {
        AddPrecondition("HasFood", true);
        AddEffect("IsSatisfied", true);
        AddEffect("HasFood", false);
    }

    protected override IEnumerator PerformAction(WorldState state)
    {
        Debug.Log("Eating... ñam ñam");
        yield return new WaitForSeconds(1f);

        Complete(state);
    }
}

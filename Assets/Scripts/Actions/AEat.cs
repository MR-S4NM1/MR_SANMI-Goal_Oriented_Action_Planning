using System.Collections;
using UnityEngine;

public class AEat : GOAPAction
{
    private void Awake()
    {
        AddPrecondition("IsAtLocation", true);
        AddEffect("IsSatisfied", true);
        AddEffect("IsAtLocation", false);
    }

    protected override IEnumerator PerformAction(WorldState state)
    {
        Debug.Log("Eating... ñam ñam");
        yield return new WaitForSeconds(1f);

        Complete(state);
    }
}

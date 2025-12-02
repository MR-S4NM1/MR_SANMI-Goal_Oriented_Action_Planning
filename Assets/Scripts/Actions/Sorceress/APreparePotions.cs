using UnityEngine;
using System.Collections;

public class APreparePotions : GOAPAction
{
    private void Awake()
    {
        AddPrecondition("HasGottenPlants", true);

        AddEffect("HasPreparedPotions", true);
    }

    protected override IEnumerator PerformAction(WorldState state)
    {
        Debug.Log("Preparing potions...");
        yield return new WaitForSeconds(2f);
        Debug.Log("Potions prepared! :P");

        Complete(state);
    }
}

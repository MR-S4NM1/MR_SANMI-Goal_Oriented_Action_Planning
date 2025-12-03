using System.Collections;
using UnityEngine;

public class AGetFood : GOAPAction
{
    [SerializeField] private Transform fridge;

    private void OnEnable()
    {
        if (animationsManager == null)
            animationsManager = GetComponent<AnimationsManager>();

        AddEffect("HasFood", true);
    }

    protected override IEnumerator PerformAction(WorldState state)
    {
        Debug.Log("Going for food...");

        while (Vector3.Distance(transform.position, fridge.position) > 1.5f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                fridge.position,
                Time.deltaTime * 3.0f);

            yield return null;
        }

        yield return new WaitForSeconds(0.5f);

        Debug.Log("Got food!");
        Complete(state);
    }
}

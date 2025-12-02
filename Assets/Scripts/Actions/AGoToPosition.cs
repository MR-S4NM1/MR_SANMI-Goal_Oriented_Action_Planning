using System.Collections;
using UnityEngine;

public class AGoToPosition : GOAPAction
{
    [SerializeField] private Transform target; 

    private void Awake()
    {
        AddEffect("IsAtTarget", true);
        cost = 1.0f;
    }

    protected override IEnumerator PerformAction(WorldState state)
    {
        Debug.Log("Going to destiny...");

        while (Vector3.Distance(transform.position, target.position) > 1.5f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                target.position,
                Time.deltaTime * 3.0f);

            yield return null;
        }

        yield return new WaitForSeconds(0.2f);

        Debug.Log("Arrived!");
        Complete(state);
    }
}

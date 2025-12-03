using UnityEngine;
using System.Collections;

public class AGoToPosition : GOAPAction
{
    [SerializeField] protected Transform target;
    [SerializeField] protected float distanceThreshold = 1.5f;
    [SerializeField] protected float movementSpeed = 1.0f;

    private void Awake()
    {
        if (animationsManager == null)
            animationsManager = GetComponent<AnimationsManager>();
    }

    protected override IEnumerator PerformAction(WorldState state)
    {
        animationsManager.AnimationFunction("Walk", true);

        while (Vector3.Distance(transform.position, target.position) > distanceThreshold)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                target.position,
                Time.deltaTime * 1.5f);

            transform.LookAt(new Vector3(target.position.x, transform.position.y, target.position.z));

            yield return null;
        }

        yield return new WaitForSeconds(0.2f);

        Complete(state);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class APatrol : GOAPAction
{
    [SerializeField] protected List<Transform> targets = new List<Transform>();
    [SerializeField] protected float distanceThreshold = 1.5f;
    [SerializeField] protected float movementSpeed = 1.0f;

    protected int currentIndex = 0;
    private void Awake()
    {
        if (animationsManager == null)
            animationsManager = GetComponent<AnimationsManager>();

        AddPrecondition("HasFinishedPatrolling", false);
        AddEffect("HasFinishedPatrolling", true);
    }


    protected override IEnumerator PerformAction(WorldState state)
    {
        animationsManager.AnimationFunction("Walk", true);

        while (true)
        {
            if (currentIndex == targets.Count) break;

            if (Vector3.Distance(transform.position, targets[currentIndex].position) > distanceThreshold)
            {
                transform.position = Vector3.MoveTowards(
                    transform.position,
                    targets[currentIndex].position,
                    Time.deltaTime * movementSpeed);

                transform.LookAt(new Vector3(targets[currentIndex].position.x, transform.position.y, targets[currentIndex].position.z));
            }
            else
            {
                animationsManager.AnimationFunction("Idle", true);

                yield return new WaitForSeconds(3.0f);

                animationsManager.AnimationFunction("Walk", true);

                currentIndex = (currentIndex + 1) % targets.Count;
            }

            yield return null;
        }

        yield return new WaitForSeconds(0.2f);

        Complete(state);
    }
}

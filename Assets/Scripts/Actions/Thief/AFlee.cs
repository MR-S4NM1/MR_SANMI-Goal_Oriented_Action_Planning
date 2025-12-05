using UnityEngine;
using System.Collections;

public class AFlee : AGoToPosition
{
    private void Start()
    {
        if (animationsManager == null)
            animationsManager = GetComponent<AnimationsManager>();

        AddPrecondition("IsAlive", true);
        AddPrecondition("TownInDanger", true);
        AddPrecondition("HasStolenItem", true);
        AddPrecondition("HasFled", false);
        AddEffect("HasFled", true);

        AddEffect("TownInDanger", false);
    }

    protected override IEnumerator PerformAction(WorldState state)
    {
        animationsManager.AnimationFunction("Walk", true);

        while (Vector3.Distance(transform.position, target.position) > distanceThreshold)
        {
            if (isCancelled || (bool)state["ThiefCaught"] == true)
            {
                currentAnimationDuration = animationsManager.GetAnimationLength("Die");
                animationsManager.AnimationFunction("Die", true);

                yield return new WaitForSeconds(currentAnimationDuration - 0.5f);

                Complete(state);
                gameObject.SetActive(false);
                yield break;
            }

            transform.position = Vector3.MoveTowards(
                transform.position,
                target.position,
                Time.deltaTime * movementSpeed);

            transform.LookAt(new Vector3(target.position.x, transform.position.y, target.position.z));

            yield return null;
        }

        yield return new WaitForSeconds(0.2f);

        Complete(state);
        gameObject.SetActive(false);
    }
}
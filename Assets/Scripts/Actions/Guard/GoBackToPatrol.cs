public class GoBackToPatrol : APatrol
{
    void Start()
    {
        if (animationsManager == null)
            animationsManager = GetComponent<AnimationsManager>();

        AddPrecondition("ThiefCaught", true);
        AddEffect("HasFinishedNewPatrolling", true);
    }
}

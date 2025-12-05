public class AAGoAfterTheThief_Guard : AGoToPosition
{
    private void Start()
    {
        if (animationsManager == null)
            animationsManager = GetComponent<AnimationsManager>();

        AddPrecondition("TownInDanger", true);
        AddPrecondition("ThiefCaught", false);
        AddEffect("GuardInRange", true);
    }

}


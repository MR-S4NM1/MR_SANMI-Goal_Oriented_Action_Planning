public class AAGoAfterTheThief_Sorceress : AGoToPosition
{
    private void Start()
    {
        if (animationsManager == null)
            animationsManager = GetComponent<AnimationsManager>();

        AddEffect("SorceressInRange", true);
    }
}
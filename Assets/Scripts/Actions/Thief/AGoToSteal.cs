public class AGoToSteal : AGoToPosition
{
    private void Start()
    {
        if (animationsManager == null)
            animationsManager = GetComponent<AnimationsManager>();

        AddPrecondition("IsAlive", true);
        AddPrecondition("HasStolenItem", false);
        AddPrecondition("TownInDanger", false);
        AddPrecondition("IsAtItemsLocation", false);
        AddEffect("IsAtItemsLocation", true);
    }
}

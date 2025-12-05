public class AGoToPreparePotions : AGoToPosition
{
    private void Start()
    {
        if (animationsManager == null)
            animationsManager = GetComponent<AnimationsManager>();

        AddPrecondition("HasCollectedPlants", true);
        AddPrecondition("IsAtPotionsCraftLocation", false);
        AddEffect("IsAtPotionsCraftLocation", true);
    }
}

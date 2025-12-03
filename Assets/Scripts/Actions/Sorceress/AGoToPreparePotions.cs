public class AGoToPreparePotions : AGoToPosition
{
    private void Awake()
    {
        if (animationsManager == null)
            animationsManager = GetComponent<AnimationsManager>();

        AddPrecondition("HasCollectedPlants", true);
        AddEffect("IsAtPotionsCraftLocation", true);
    }
}

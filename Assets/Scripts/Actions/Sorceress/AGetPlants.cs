public class AGetPlants : AGoToPosition
{
    private void Start()
    {
        if (animationsManager == null)
            animationsManager = GetComponent<AnimationsManager>();

        AddPrecondition("IsAtPlantsLocation", false);
        AddEffect("IsAtPlantsLocation", true);

        AddEffect("HasPreparedPotions", false);
    }
}

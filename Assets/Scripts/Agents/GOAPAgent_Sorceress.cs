public class GOAPAgent_Sorceress : GOAPAgent
{
    private void Awake()
    {
        InitializeAgent();
    }

    protected override void PrepareWorldState()
    {
        worldState["IsAtPlantsLocation"] = false;
        worldState["HasCollectedPlants"] = false;
        worldState["IsAtPotionsCraftLocation"] = false;
        worldState["HasPreparedPotions"] = false;

        worldState["TownInDanger"] = false;
    }

    protected override void PrepareGoals()
    {
        var collectPlantsGoal = new GOAPGoal("CollectPlants", 1.0f);
        collectPlantsGoal.desiredState["HasCollectedPlants"] = true;
        goals.Add(collectPlantsGoal);

        var preparePotionsGoal = new GOAPGoal("PreparePotions", 1.0f);
        preparePotionsGoal.desiredState["HasPreparedPotions"] = true;
        goals.Add(preparePotionsGoal);

        reactiveKeys.Add("TownInDanger");  // si esto cambia, replanear
        reactiveKeys.Add("EnemyDetected"); // si LADRÓN aparece, replanear
    }
}

public class GOAPAgent_Sorceress : GOAPAgent
{
    private void Start()
    {
        StartCoroutine(DelayedInitialize());
    }

    protected override void PrepareWorldState()
    {
        // Sus tareas normales
        worldState["IsAtPlantsLocation"] = false;
        worldState["HasCollectedPlants"] = false;
        worldState["IsAtPotionsCraftLocation"] = false;
        worldState["HasPreparedPotions"] = false;

        // GLOBALS
        worldState["TownInDanger"] = false;
        worldState["ThiefCaught"] = false;

        // LOCAL: rango exclusivo de ella
        worldState["SorceressInRange"] = false;
    }

    protected override void PrepareGoals()
    {
        var collectGoal = new GOAPGoal("CollectPlants", 1.0f);
        collectGoal.desiredState["HasCollectedPlants"] = true;
        goals.Add(collectGoal);

        var potionsGoal = new GOAPGoal("PreparePotions", 1.0f);
        potionsGoal.desiredState["HasPreparedPotions"] = true;
        goals.Add(potionsGoal);

        var catchGoal = new GOAPGoal("CatchThief", 1.0f);
        catchGoal.desiredState["ThiefCaught"] = true;
        goals.Add(catchGoal);

        reactiveKeys.Add("TownInDanger");
    }
}

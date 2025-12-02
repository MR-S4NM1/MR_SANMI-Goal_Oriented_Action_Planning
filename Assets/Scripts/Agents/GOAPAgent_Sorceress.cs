public class GOAPAgent_Sorceress : GOAPAgent
{
    private void Awake()
    {
        InitializeAgent();
    }

    protected override void InitializeAgent()
    {
        base.InitializeAgent();
    }

    protected override void PrepareWorldState()
    {
        worldState["HasGottenPlants"] = false;
        worldState["HasPreparedPotions"] = false;

        worldState["IsTired"] = true;
        worldState["IsWellRest"] = false;
    }

    protected override void PrepareGoals()
    {
        var preparePotionsGoal = new GOAPGoal("PreparePotions", 1.0f);
        preparePotionsGoal.desiredState["HasPreparedPotions"] = true;
        goals.Add(preparePotionsGoal);

        print($"Meta: {preparePotionsGoal.name}");

        var sleepGoal = new GOAPGoal("Sleep", 1.0f);
        sleepGoal.desiredState["IsWellRest"] = true;
        goals.Add(sleepGoal);
    }
}

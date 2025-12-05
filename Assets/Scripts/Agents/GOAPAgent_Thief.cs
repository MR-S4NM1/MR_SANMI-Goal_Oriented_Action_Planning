public class GOAPAgent_Thief : GOAPAgent
{
    private void Start()
    {
        StartCoroutine(DelayedInitialize());
    }

    protected override void PrepareWorldState()
    {
        // Sus estados locales
        worldState["IsAtItemsLocation"] = false;
        worldState["HasStolenItem"] = false;
        worldState["HasFled"] = false;
        worldState["IsAlive"] = true;

        // GLOBALS
        worldState["TownInDanger"] = false;
        worldState["ThiefCaught"] = false;  // él NO cambia esto, solo lo lee
    }

    protected override void PrepareGoals()
    {
        var stealGoal = new GOAPGoal("Steal", 5.0f);
        stealGoal.desiredState["HasStolenItem"] = true;
        goals.Add(stealGoal);

        var fleeGoal = new GOAPGoal("Flee", 10.0f);
        fleeGoal.desiredState["HasFled"] = true;
        goals.Add(fleeGoal);

        var deathGoal = new GOAPGoal("Die", 10.0f);
        deathGoal.desiredState["IsAlive"] = false;
        goals.Add(deathGoal);

        reactiveKeys.Add("TownInDanger");
        reactiveKeys.Add("ThiefCaught");
    }
}

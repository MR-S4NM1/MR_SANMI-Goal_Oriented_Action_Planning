public class GOAPAgent_Guard : GOAPAgent
{
    private void Start()
    {
        StartCoroutine(DelayedInitialize());
    }

    protected override void PrepareWorldState()
    {
        // Estado base
        worldState["HasFinishedPatrolling"] = false;

        // GLOBALS
        worldState["TownInDanger"] = false;
        worldState["ThiefCaught"] = false;

        // LOCALES DEL GUARDIA
        worldState["GuardInRange"] = false;
    }

    protected override void PrepareGoals()
    {
        var patrollingGoal = new GOAPGoal("Patrol", 3.0f);
        patrollingGoal.desiredState["HasFinishedPatrolling"] = true;
        goals.Add(patrollingGoal);

        var catchGoal = new GOAPGoal("CatchThief", 3.0f);
        catchGoal.desiredState["ThiefCaught"] = true;
        goals.Add(catchGoal);

        reactiveKeys.Add("TownInDanger");
    }
}

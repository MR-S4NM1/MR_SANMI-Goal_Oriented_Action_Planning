public class GOAPAgent_Guard : GOAPAgent
{
    private void Awake()
    {
        InitializeAgent();
    }

    protected override void PrepareWorldState()
    {
        worldState["HasFinishedPatrolling"] = false;

        worldState["TownInDanger"] = false;
    }

    protected override void PrepareGoals()
    {
        var patrollingGoal = new GOAPGoal("Patrol", 1.0f);
        patrollingGoal.desiredState["HasFinishedPatrolling"] = true;
        goals.Add(patrollingGoal);

        reactiveKeys.Add("TownInDanger");  // si esto cambia, replanear
        reactiveKeys.Add("EnemyDetected"); // si LADRÓN aparece, replanear
    }
}

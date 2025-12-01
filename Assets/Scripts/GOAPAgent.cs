using System.Collections.Generic;
using UnityEngine;

public class GOAPAgent : MonoBehaviour
{
    public List<GOAPAction> actions = new List<GOAPAction>();
    public List<GOAPGoal> goals = new List<GOAPGoal>();
    public GOAPPlanner planner = new GOAPPlanner();

    public Queue<GOAPAction> CurrentPlan { get; set; }
    public WorldState worldState = new WorldState();

    private IAgentState currentState;

    private void Start()
    {
        // Ejemplo de estado del mundo inicial
        worldState["HasFood"] = false;
        worldState["IsSatisfied"] = false;

        worldState["IsTired"] = true;
        worldState["IsWellRest"] = false;

        worldState.OnWorldStateChanged += OnWorldStateChanged;

        // Agregar acciones anexas al GameObject
        actions.AddRange(GetComponents<GOAPAction>());

        // Goals
        var eatGoal = new GOAPGoal("Eat", 2f);
        eatGoal.desiredState["IsSatisfied"] = true;
        goals.Add(eatGoal);

        var sleepGoal = new GOAPGoal("Sleep", 1f);
        sleepGoal.desiredState["IsWellRest"] = true;
        goals.Add(sleepGoal);

        ChangeState(new PlanningState(this));
    }

    public void ChangeState(IAgentState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState.Enter();
    }

    public GOAPGoal ChooseGoal()
    {
        GOAPGoal bestGoal = null;

        foreach (var goal in goals)
        {
            if (!PlannerIsSatisfied(worldState, goal))
            {
                if (bestGoal == null || goal.priority > bestGoal.priority)
                    bestGoal = goal;
            }
        }

        return bestGoal;
    }

    bool PlannerIsSatisfied(WorldState state, GOAPGoal goal)
    {
        foreach (var kv in goal.desiredState)
        {
            if (!state.ContainsKey(kv.Key)) return false;
            if (!state[kv.Key].Equals(kv.Value)) return false;
        }

        return true;
    }

    private void OnWorldStateChanged(string key, object value)
    {
        Debug.Log($"World changed: {key} = {value}. Replanning...");
        ChangeState(new ReplanningState(this));
    }
}

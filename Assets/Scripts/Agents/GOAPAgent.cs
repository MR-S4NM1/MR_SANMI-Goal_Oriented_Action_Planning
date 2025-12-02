using System.Collections.Generic;
using UnityEngine;

public class GOAPAgent : MonoBehaviour
{
    public List<GOAPAction> actions = new List<GOAPAction>();
    public List<GOAPGoal> goals = new List<GOAPGoal>();
    public GOAPPlanner planner = new GOAPPlanner();

    public Queue<GOAPAction> CurrentPlan { get; set; }
    public WorldState worldState = new WorldState();

    protected IAgentState currentState;

    private void Start()
    {
        InitializeAgent();
    }

    protected virtual void InitializeAgent()
    {
        PrepareWorldState();
        worldState.OnWorldStateChanged += OnWorldStateChanged;

        // Agregar acciones anexas al GameObject
        actions.AddRange(GetComponents<GOAPAction>());
        PrepareGoals();

        ChangeState(new PlanningState(this));
    }

    protected virtual void PrepareWorldState()
    {

    }

    protected virtual void PrepareGoals()
    {

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

    protected virtual bool PlannerIsSatisfied(WorldState state, GOAPGoal goal)
    {
        foreach (var kv in goal.desiredState)
        {
            if (!state.ContainsKey(kv.Key)) return false;
            if (!state[kv.Key].Equals(kv.Value)) return false;
        }

        return true;
    }

    protected virtual void OnWorldStateChanged(string key, object value)
    {
        Debug.Log($"World changed: {key} = {value}. Replanning...");
        ChangeState(new ReplanningState(this));
    }
}

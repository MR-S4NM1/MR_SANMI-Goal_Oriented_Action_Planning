using System.Collections.Generic;
using UnityEngine;

public class GOAPAgent : MonoBehaviour
{
    public List<GOAPAction> actions = new List<GOAPAction>();
    public List<GOAPGoal> goals = new List<GOAPGoal>();
    public GOAPPlanner planner = new GOAPPlanner();

    public Queue<GOAPAction> CurrentPlan { get; set; }
    public WorldState worldState = new WorldState();

    public HashSet<string> reactiveKeys = new HashSet<string>();

    protected IAgentState currentState;

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

    protected bool PlannerIsSatisfied(WorldState state, GOAPGoal goal)
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
        // Si la clave NO es importante para este NPC, ignórala.
        if (!reactiveKeys.Contains(key))
            return;

        // Si estoy ejecutando un plan, NO replanear hasta que termine.
        if (currentState is ExecutingPlanState)
            return;

        Debug.Log($"{name} reacts to world change: {key} = {value}");
        ChangeState(new ReplanningState(this));
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GOAPAgent : MonoBehaviour
{
    public List<GOAPAction> actions = new List<GOAPAction>();
    public List<GOAPGoal> goals = new List<GOAPGoal>();
    public GOAPPlanner planner = new GOAPPlanner();

    public bool isInitialized = false;

    public GOAPAction currentAction;
    public Coroutine currentActionRoutine;

    public Queue<GOAPAction> CurrentPlan { get; set; }

    public WorldState worldState => GlobalWorldState.Instance.State;

    public HashSet<string> reactiveKeys = new HashSet<string>();

    protected IAgentState currentState;

    void Start()
    {
        StartCoroutine(DelayedInitialize());
    }

    protected IEnumerator DelayedInitialize()
    {
        // Esperar 2 frames: más robusto contra race conditions en escenas complejas
        yield return null;
        yield return null;

        // seguridad: si alguien ya inicializó, no lo repitamos
        if (!isInitialized)
            InitializeAgent();
    }

    protected virtual void InitializeAgent()
    {
        if (isInitialized) return;

        // 1) Preparar worldstate ANTES de subscribir
        PrepareWorldState();

        // 2) Agregar acciones
        actions.Clear();
        actions.AddRange(GetComponents<GOAPAction>());

        // 3) Preparar goals
        PrepareGoals();

        // 4) Subscribir al ESTADO GLOBAL
        GlobalWorldState.Instance.State.OnWorldStateChanged += OnWorldStateChanged;

        isInitialized = true;
        ChangeState(new PlanningState(this));
    }

    private void OnDestroy()
    {
        if (GlobalWorldState.Instance != null)
        {
            GlobalWorldState.Instance.State.OnWorldStateChanged -= OnWorldStateChanged;
        }
    }

    protected virtual void PrepareWorldState()
    {

    }

    protected virtual void PrepareGoals()
    {

    }

    public virtual void ChangeState(IAgentState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState?.Enter();
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
        if (!isInitialized)
            return;

        if (!reactiveKeys.Contains(key))
            return;

        var globalState = GlobalWorldState.Instance.State;
        if (globalState.ContainsKey(key) && globalState[key].Equals(value))
            return;

        Debug.Log($"{name} REACCIONA a: {key} = {value}");

        StopAllCoroutines();

        if (currentAction != null)
        {
            currentAction.Cancel();
            currentAction = null;
        }

        currentActionRoutine = null;

        CurrentPlan = null;

        Debug.Log($"{name} forzando replanning..."); 
        ChangeState(new ReplanningState(this));
    }
}

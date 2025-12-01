using System.Collections.Generic;
using UnityEngine;

public class GOAPPlanner
{
    public Queue<GOAPAction> Plan(
        WorldState worldState,
        List<GOAPAction> availableActions,
        GOAPGoal goal)
    {
        List<GOAPAction> plan = new List<GOAPAction>();

        // Copiamos el estado inicial
        WorldState currentState = new WorldState();
        foreach (var kv in worldState)
        {
            currentState[kv.Key] = kv.Value;
        }

        int safety = 0;

        while (!GoalSatisfied(currentState, goal.desiredState) && safety < 20)
        {
            safety++;

            GOAPAction bestAction = null;
            bool foundImprovingAction = false; // ¿ya encontré una acción que acerque directamente al goal?

            foreach (var action in availableActions)
            {
                // 1) ¿Se cumplen sus precondiciones?
                if (!action.ArePreconditionsMet(currentState))
                    continue;

                // 2) ¿Sus efectos cambian algo del estado? (si no, la ignoramos)
                bool changesState = false;
                foreach (var effect in action.Effects)
                {
                    if (!currentState.ContainsKey(effect.Key) ||
                        !currentState[effect.Key].Equals(effect.Value))
                    {
                        changesState = true;
                        break;
                    }
                }

                if (!changesState)
                    continue;

                // 3) ¿Esta acción mejora directamente el goal?
                bool improvesGoal = false;
                foreach (var effect in action.Effects)
                {
                    if (goal.desiredState.ContainsKey(effect.Key) &&
                        effect.Value.Equals(goal.desiredState[effect.Key]))
                    {
                        improvesGoal = true;
                        break;
                    }
                }

                // 4) Estrategia de elección:
                //    - Si encontramos una acción que mejora el goal, la priorizamos.
                //    - Si ninguna mejora el goal todavía, elegimos cualquiera que cambie el estado.
                if (improvesGoal)
                {
                    if (!foundImprovingAction || action.cost < bestAction.cost)
                    {
                        bestAction = action;
                        foundImprovingAction = true;
                    }
                }
                else if (!foundImprovingAction) // solo consideramos "acciones de apoyo" si no hay una mejor
                {
                    if (bestAction == null || action.cost < bestAction.cost)
                    {
                        bestAction = action;
                    }
                }
            }

            if (bestAction == null)
            {
                Debug.LogWarning("Planner: no encontré ninguna acción aplicable.");
                return null;
            }

            // Aplicamos los efectos de la acción al estado simulado
            foreach (var effect in bestAction.Effects)
            {
                currentState[effect.Key] = effect.Value;
            }

            plan.Add(bestAction);
        }

        if (!GoalSatisfied(currentState, goal.desiredState))
        {
            Debug.LogWarning("Planner: no se pudo alcanzar el goal.");
            return null;
        }

        Debug.Log("Planner: plan generado con " + plan.Count + " acciones.");
        return new Queue<GOAPAction>(plan);
    }

    private bool GoalSatisfied(WorldState state, Dictionary<string, object> goalState)
    {
        foreach (var g in goalState)
        {
            if (!state.ContainsKey(g.Key))
                return false;

            if (!state[g.Key].Equals(g.Value))
                return false;
        }
        return true;
    }
}
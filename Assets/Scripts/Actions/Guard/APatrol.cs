using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class APatrol : GOAPAction
{
    [SerializeField] protected List<Transform> targets = new List<Transform>();
    [SerializeField] protected float distanceThreshold = 1.5f;
    [SerializeField] protected float movementSpeed = 1.0f;
    [SerializeField] protected float waitTimeAtWaypoint = 3.0f;

    protected int currentIndex = 0;
    protected bool isWaiting = false;

    private void Start()
    {
        if (animationsManager == null)
            animationsManager = GetComponent<AnimationsManager>();

        AddPrecondition("TownInDanger", false);
        AddEffect("HasFinishedPatrolling", true);
    }

    protected override IEnumerator PerformAction(WorldState state)
    {
        Debug.Log($"[APatrol] Iniciando patrulla con {targets.Count} waypoints");

        // 1. Encontrar el waypoint MÁS CERCANO para empezar
        currentIndex = FindNearestWaypointIndex();
        animationsManager.AnimationFunction("Walk", true);

        // 2. Patrullar continuamente (es un bucle infinito hasta ser cancelado)
        while (!isCancelled)
        {
            if (targets.Count == 0)
            {
                Debug.LogError("[APatrol] No hay waypoints asignados!");
                yield break;
            }

            if (isCancelled || (bool)state["TownInDanger"] == true)
            {
                Complete(state);
                yield break;
            }

            Transform currentTarget = targets[currentIndex];
            float distance = Vector3.Distance(transform.position, currentTarget.position);

            // 3. Si NO hemos llegado al waypoint, movernos hacia él
            if (distance > distanceThreshold)
            {
                // Movimiento
                transform.position = Vector3.MoveTowards(
                    transform.position,
                    currentTarget.position,
                    Time.deltaTime * movementSpeed);

                // Rotación
                transform.LookAt(new Vector3(targets[currentIndex].position.x, transform.position.y, 
                    targets[currentIndex].position.z));


                yield return null;
                continue; // Volver al inicio del while
            }

            // 4. SI hemos llegado al waypoint
            if (!isWaiting)
            {
                Debug.Log($"[APatrol] Llegó al waypoint {currentIndex}. Esperando {waitTimeAtWaypoint} segundos...");

                animationsManager.AnimationFunction("Idle", true);
                isWaiting = true;

                yield return new WaitForSeconds(waitTimeAtWaypoint);

                // 5. Pasar al siguiente waypoint
                currentIndex = (currentIndex + 1) % targets.Count;
                Debug.Log($"[APatrol] Siguiente waypoint: {currentIndex}");

                animationsManager.AnimationFunction("Walk", true);
                isWaiting = false;
            }

            yield return null;
        }

        // 7. Completar (en teoría nunca llega aquí porque es bucle infinito)
        Debug.Log("[APatrol] Patrulla completada (¿cómo llegó aquí?)");
        Complete(state);
    }

    protected int FindNearestWaypointIndex()
    {
        if (targets.Count == 0) return 0;

        int nearestIndex = 0;
        float nearestDistance = float.MaxValue;

        for (int i = 0; i < targets.Count; i++)
        {
            if (targets[i] == null) continue;

            float distance = Vector3.Distance(transform.position, targets[i].position);
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestIndex = i;
            }
        }

        Debug.Log($"[APatrol] Waypoint más cercano: {nearestIndex} (distancia: {nearestDistance})");
        return nearestIndex;
    }

    public override void Cancel()
    {
        base.Cancel();
        isWaiting = false;
        animationsManager.AnimationFunction("Idle", true);
    }
}
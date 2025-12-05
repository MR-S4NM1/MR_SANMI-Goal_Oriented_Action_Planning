using UnityEngine;

public class GlobalWorldState : MonoBehaviour
{
    public static GlobalWorldState Instance { get; private set; }

    public WorldState State { get; private set; }

    private void Awake()
    {
        if (Instance == null && Instance != this)
        {
            Instance = this;
        }

        State = new WorldState();
    }
}
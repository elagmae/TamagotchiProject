using System;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    public static StateManager Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void AddToState(AnimalLevel level, float amount)
    {
        AnimalState state = RoomManager.Instance.RoomData.AnimalStates[(int)level];
        state.Value += amount;
        RoomManager.Instance.RoomData.AnimalStates[(int)level] = state;
    }

    public void RemoveFromState(AnimalLevel level, float amount)
    {
        AnimalState state = RoomManager.Instance.RoomData.AnimalStates[(int)level];
        state.Value -= amount;
        RoomManager.Instance.RoomData.AnimalStates[(int)level] = state;
    }

}

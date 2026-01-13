using System;
using UnityEngine;

public static class StatesHandler
{
    public static event Action<AnimalState, float> UpdateStatesUI;

    public static void ModifyState(AnimalLevel level, float addedValue)
    {
        AnimalState state = RoomManager.CurrentRoomData.AnimalStates[(int)level];
        state.Value += addedValue;

        RoomManager.CurrentRoomData.AnimalStates[(int)level] = state;

        UpdateStatesUI?.Invoke(RoomManager.CurrentRoomData.AnimalStates[(int)level], state.Value);
    }
}

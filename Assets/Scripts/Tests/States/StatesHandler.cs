using System;
using UnityEngine;

public static class StatesHandler
{
    public static event Action<AnimalState, float> UpdateStatesUI;

    public static void ModifyState(AnimalLevel level, float addedValue)
    {
        AnimalState state = RoomUpdater.CurrentRoomData.AnimalStates[(int)level];
        state.Value += addedValue;

        RoomUpdater.CurrentRoomData.AnimalStates[(int)level] = state;

        UpdateStatesUI?.Invoke(RoomUpdater.CurrentRoomData.AnimalStates[(int)level], state.Value);
    }
}

using System;
using System.Collections.Generic;
using Unity.Services.CloudSave;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public static class SaveManager
{
    public static event Action<string, RoomData, List<int>> OnRoomUpdated;

    private static RoomData _save;

    public static async void GetRoom()
    {
        try
        {
            var playerData = await CloudSaveService.Instance.Data.Player.LoadAsync(new HashSet<string> { "CurrentRoom", "CurrentDays" });
            string id = playerData["CurrentRoom"].Value.GetAs<string>();
            string days = playerData["CurrentDays"].Value.GetAs<string>();

            var customItemData = await CloudSaveService.Instance.Data.Custom.LoadAllAsync(id);
            var value = customItemData[id].Value;

            _save = value.GetAs<RoomData>();

            RoomUpdater.CurrentRoomId = id;
            RoomUpdater.CurrentRoomData = _save;
            RoomUpdater.CurrentActiveDays = JsonUtility.FromJson<List<int>>(days);

            OnRoomUpdated?.Invoke(RoomUpdater.CurrentRoomId, RoomUpdater.CurrentRoomData, RoomUpdater.CurrentActiveDays);
        }

        catch (Exception e)
        {
            Debug.Log(e);
        }

    }
}

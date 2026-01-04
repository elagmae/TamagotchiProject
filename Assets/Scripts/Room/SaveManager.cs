using System;
using System.Collections.Generic;
using Unity.Services.CloudSave;
using UnityEngine;

public static class SaveManager
{
    public static event Action<string, RoomData> OnRoomUpdated;

    private static string _json;

    public static async void GetRoom()
    {
        try
        {
            var playerData = await CloudSaveService.Instance.Data.Player.LoadAsync(new HashSet<string> { "CurrentRoom" });
            string id = playerData["CurrentRoom"].Value.GetAs<string>();

            var customItemData = await CloudSaveService.Instance.Data.Custom.LoadAllAsync(id);
            var value = customItemData[id].Value;
            _json = value.GetAs<string>();

            RoomManager.CurrentRoomId = id;
            RoomManager.CurrentRoomData = JsonUtility.FromJson<RoomData>(_json);

            OnRoomUpdated?.Invoke(RoomManager.CurrentRoomId, RoomManager.CurrentRoomData);
        }

        catch(Exception e)
        {
            Debug.Log("No Room found for this player.");
        }

    }
}

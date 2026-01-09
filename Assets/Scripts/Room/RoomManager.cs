using System;
using System.Collections.Generic;
using Unity.Services.CloudCode;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public static class RoomManager
{
    public static string CurrentRoomId { get; set; } = string.Empty;
    public static RoomData CurrentRoomData { get; set; }
    public static string CurrentPlayerAnimalName { get; set; }
    public static List<int> CurrentActiveDays { get; set; }

    public static async void UpdateRoom(string roomId, string json, string otherPlayer, string animalName, string playerId, string _otherDays, string activeDays)
    {
        try
        {
            var parameters = new Dictionary<string, object>
            {
                { "file", json },
                { "room", roomId },
                {"animalName", animalName },
                {"otherPlayer", otherPlayer },
                {"playerId", playerId },
                {"activeDays", activeDays },
                {"otherDays", _otherDays }
            };

            var response = await CloudCodeService.Instance.CallEndpointAsync<object>("ItemSetter", parameters);

            if (!(bool)response) return;

            CurrentRoomId = roomId;
            CurrentRoomData = JsonUtility.FromJson<RoomData>(json);
            CurrentActiveDays = JsonUtility.FromJson<List<int>>(activeDays);

            Debug.Log("Values updated successfully !");
        }

        catch (Exception e)
        {
            Debug.LogError(e);
            Debug.LogWarning("Failed to update, check your assignations and try again !");
        }
    }
}

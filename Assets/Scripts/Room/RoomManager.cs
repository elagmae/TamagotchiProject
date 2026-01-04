using System;
using System.Collections.Generic;
using Unity.Services.CloudCode;
using UnityEngine;

public static class RoomManager
{
    public static string CurrentRoomId { get; set; } = string.Empty;
    public static RoomData CurrentRoomData { get; set; }
    public static string CurrentPlayerAnimalName { get; set; }

    public static async void UpdateRoom(string roomId, string json, string otherPlayer, string animalName, string playerId)
    {
        try
        {
            var parameters = new Dictionary<string, object>
            {
                { "file", json },
                { "room", roomId },
                {"animalName", animalName },
                {"otherPlayer", otherPlayer },
                {"playerId", playerId }
            };

            var response = await CloudCodeService.Instance.CallEndpointAsync<object>("ItemSetter", parameters);

            CurrentRoomId = roomId;
            CurrentRoomData = JsonUtility.FromJson<RoomData>(json);

            Debug.Log("Values updated successfully !");
        }

        catch (Exception e)
        {
            Debug.LogError(e);
            Debug.LogWarning("Failed to update, check your assignations andd try again !");
        }
    }
}

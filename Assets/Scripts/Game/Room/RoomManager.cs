using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.CloudCode;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public static RoomManager Instance;
    public bool CanPlay { get; set; }
    public RoomData RoomData { get; set; } = new() { AnimalStates = new(), AnimalName = "" , ParentNote = "", LastConnection = DateTime.Now};
    public string RoomId { get; set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public async void UpdateRoom()
    {
        await Task.Yield();

        RoomData data = RoomData;
        data.LastConnection = DateTime.Now;
        RoomData = data;

        var save = new Dictionary<string, object>
        {
            {"id", RoomId },
            {"file", RoomData}
        };

        try
        {
            await CloudCodeService.Instance.CallEndpointAsync<object>("RoomUpdater", save);
        }

        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }

    public async void CreateRoom(string roomId, string json, string otherPlayer, string animalName, string playerId, string _otherDays, string activeDays)
    {
        try
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                {"file", json },
                {"room", roomId },
                {"animalName", animalName },
                {"otherPlayer", otherPlayer },
                {"playerId", playerId },
                {"activeDays", activeDays },
                {"otherDays", _otherDays }
            };

            object response = await CloudCodeService.Instance.CallEndpointAsync<object>("ItemSetter", parameters);

            Dictionary<string, object> id = new Dictionary<string, object>
            {
                { "playerId", playerId }
            };

            CanPlay = await CloudCodeService.Instance.CallEndpointAsync<bool>("DayAnalyzer", id);

            string sceneName = CanPlay ? "AnimalRoom" : "WaitingRoom";

            RoomId = roomId;
            RoomData = JsonUtility.FromJson<RoomData>(json);

            SceneLoadManager.Instance.LoadScene(sceneName);
        }

        catch (Exception e)
        {
            Debug.LogError(e);
        }
    }

    private void OnApplicationQuit()
    {
        try
        {
            if (!Unity.Services.Authentication.AuthenticationService.Instance.IsSignedIn) return;
            UpdateRoom();
        }

        catch
        {

        }
    }
}

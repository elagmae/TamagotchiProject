using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using Unity.Services.CloudCode;
using UnityEngine;
using UnityEngine.UI;

public class RoomAssignationHandler : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _animalName;
    [SerializeField]
    private List<Image> _states = new();
    [SerializeField]
    private GameObject _roomPanel;
    [SerializeField]
    private GameObject _block;

    private bool _hasData = false;

    private void Awake()
    {
        SaveManager.OnRoomUpdated += AssignRoomInfos;
    }

    private void Update()
    {
        if (!_hasData) return;
        for (int i = 0; i < _states.Count; i++)
        {
            var state = RoomUpdater.CurrentRoomData.AnimalStates[i];
            state.Value = _states[i].fillAmount * 100f;
            RoomUpdater.CurrentRoomData.AnimalStates[i] = state;
        }

        print(RoomUpdater.CurrentRoomData.AnimalStates[1].Value);
    }

    public void OpenRoom()
    {
        AssignRoomInfos(RoomUpdater.CurrentRoomId, RoomUpdater.CurrentRoomData, RoomUpdater.CurrentActiveDays);
    }

    public void AssignRoomInfos(string id, RoomData data, List<int> activeDays)
    {
        _roomPanel.SetActive(true);
        _animalName.text = data.AnimalName;

        for(int i = 0; i < data.AnimalStates.Count; i++)
        {
            _states[i].fillAmount = data.AnimalStates[i].Value / 100f;
        }

        _hasData = true;

        DaysVerif();
    }

    private void OnApplicationQuit()
    {
        SaveRoom();
    }

    public async void DaysVerif()
    {

        try
        {
            var parameters = new Dictionary<string, object>
            {
                { "playerId", Unity.Services.Authentication.AuthenticationService.Instance.PlayerId }
            };

            await Task.Delay(500);
            var response = await CloudCodeService.Instance.CallEndpointAsync<object>("DayAnalyzer", parameters);
            print(response);
            _block.SetActive(!(bool)response);
            Debug.Log("Values updated successfully !");
        }

        catch (Exception e)
        {
            Debug.LogError(e);
            Debug.LogWarning("Failed to update, check your assignations and try again !");
        }
    }

    public async void SaveRoom()
    {
        RoomData data = RoomUpdater.CurrentRoomData;
        data.LastConnection = DateTime.Now;
        RoomUpdater.CurrentRoomData = data;

        print(RoomUpdater.CurrentRoomData.AnimalStates[1].Value);
        var save = new Dictionary<string, object>
        {
            {"id", RoomUpdater.CurrentRoomId },
            {"file", RoomUpdater.CurrentRoomData}
        };

        try
        {
            await CloudCodeService.Instance.CallEndpointAsync<object>("RoomUpdater", save);
            Debug.Log("Updated");
        }

        catch (Exception e)
        {
            Debug.LogError($"Failed to save room: {e}");
        }

    }
}

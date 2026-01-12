using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using Unity.Services.CloudCode;
using Unity.Services.Core;
using UnityEngine;

public class RoomAssignationHandler : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _animalName;
    [SerializeField]
    private List<TextMeshProUGUI> _animalStatesTexts;
    [SerializeField]
    private GameObject _roomPanel;
    [SerializeField]
    private GameObject _block;

    private void Awake()
    {
        SaveManager.OnRoomUpdated += AssignRoomInfos;
    }

    public void OpenRoom()
    {
        AssignRoomInfos(RoomManager.CurrentRoomId, RoomManager.CurrentRoomData, RoomManager.CurrentActiveDays);
    }

    public void AssignRoomInfos(string id, RoomData data, List<int> activeDays)
    {
        _roomPanel.SetActive(true);
        _animalName.text = data.AnimalName;

        for(int i = 0; i < data.AnimalStates.Count; i++)
        {
            _animalStatesTexts[i].text = $"{data.AnimalStates[i].Level} : {data.AnimalStates[i].Value}%";
        }

        DaysVerif();
    }

    public async void DaysVerif()
    {

        try
        {
            var parameters = new Dictionary<string, object>
            {
                { "playerId", Unity.Services.Authentication.AuthenticationService.Instance.PlayerId }
            };

            await Task.Delay(100);
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
}

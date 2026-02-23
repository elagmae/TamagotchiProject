using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using Unity.Services.CloudCode;
using UnityEngine;

public class RoomCreationBehaviour : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField _animalDisplay;

    [SerializeField]
    private TMP_InputField _otherPlayerId;

    [SerializeField]
    private GameObject _loadingPanel;

    private ActiveDaysHandler _planning;

    private void Awake()
    {
        TryGetComponent(out _planning);
    }

    public async void AddRoom()
    {
        await Task.Yield();

        try
        {
            Dictionary<string, object> id = new()
            {
                { "playerId", _otherPlayerId.text.Trim() }
            };

            bool response = await CloudCodeService.Instance.CallEndpointAsync<bool>("RoomChecker", id);

            await Task.Delay(100);

            if (!response)
            {
                Debug.LogWarning("There is a problem with your partner's id. The other player may already own a pet, or their id is incorrect. Try with another id.");
                return;
            }

            if (_animalDisplay.text == string.Empty || _otherPlayerId.text == string.Empty)
            {
                Debug.LogWarning("Values aren't all assigned !");
                return;
            }

            _loadingPanel.SetActive(true);

            RoomManager.Instance.RoomData = new RoomData()
            {
                AnimalName = _animalDisplay.text,
                AnimalStates = new List<AnimalState>()
                {
                    new AnimalState { Level = AnimalLevel.HUNGER, Value = 1f},
                    new AnimalState { Level = AnimalLevel.HYGIENE, Value = 1f },
                    new AnimalState { Level = AnimalLevel.FUN, Value = 1f },
                    new AnimalState { Level = AnimalLevel.SLEEP, Value = 1f }
                },
                LastConnection = DateTime.Now,
                IsAsleep = false,
                Parents = new string[]
                {
                    Unity.Services.Authentication.AuthenticationService.Instance.PlayerId,
                    _otherPlayerId.text
                }
            };

            await RoomManager.Instance.CreateRoom
            (
                CreateRoomId(),
                JsonUtility.ToJson(RoomManager.Instance.RoomData),  
                _otherPlayerId.text, 
                _animalDisplay.text,
                Unity.Services.Authentication.AuthenticationService.Instance.PlayerId,
                JsonUtility.ToJson(_planning.GetDays()[1]), 
                JsonUtility.ToJson(_planning.GetDays()[0])
            );
        }

        catch(Exception e)
        {
            Debug.LogException(e);
        }

    }

    public string CreateRoomId()
    {
        return "Room_" + System.Guid.NewGuid().ToString().Trim();
    }
}

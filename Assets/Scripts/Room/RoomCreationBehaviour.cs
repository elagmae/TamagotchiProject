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
    [SerializeField]
    private TextMeshProUGUI _errorMsg;
    private ActiveDaysHandler _planning;
    [SerializeField]
    private GameObject _loadingScreen;

    private void Awake()
    {
        TryGetComponent(out _planning);
    }

    public async void AddRoom()
    {
        await Task.Yield();

        try
        {
            if (_animalDisplay.text == string.Empty || _otherPlayerId.text == string.Empty || _planning.GetDays()[0].Day.Count < 1)
            {
                _errorMsg.text = "Values aren't all assigned !";
                _errorMsg.transform.parent.transform.parent.gameObject.SetActive(true);
                return;
            }

            _errorMsg.transform.parent.transform.parent.gameObject.SetActive(false);
            Dictionary<string, object> id = new()
            {
                { "playerId", _otherPlayerId.text.Trim() }
            };

            bool response = await CloudCodeService.Instance.CallEndpointAsync<bool>("RoomChecker", id);

            await Task.Delay(100);

            if (!response)
            {
                _errorMsg.text = "There is a problem with your partner's id. The other player may already own a pet, or their id is incorrect. Try with another id.";
                _errorMsg.transform.parent.transform.parent.gameObject.SetActive(true);
                return;
            }

            _loadingPanel.SetActive(true);

            RoomManager.Instance.RoomData = new RoomData()
            {
                AnimalName = _animalDisplay.text,
                AnimalStates = new List<AnimalState>()
                {
                    new AnimalState { Level = AnimalLevel.HUNGER, Value = 0.65f},
                    new AnimalState { Level = AnimalLevel.HYGIENE, Value = 0.65f },
                    new AnimalState { Level = AnimalLevel.FUN, Value = 0.65f },
                    new AnimalState { Level = AnimalLevel.SLEEP, Value = 0.65f }
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

            _loadingScreen.SetActive(true);
        }

        catch(Exception e)
        {
            _errorMsg.text = e.Message;
            _errorMsg.transform.parent.transform.parent.gameObject.SetActive(true);
        }

    }

    public string CreateRoomId()
    {
        return "Room_" + System.Guid.NewGuid().ToString().Trim();
    }
}

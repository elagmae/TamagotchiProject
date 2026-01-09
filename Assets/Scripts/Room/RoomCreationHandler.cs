using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using Unity.Services.CloudCode;
using UnityEngine;

public class RoomCreationHandler : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField _otherID;

    [SerializeField]
    private TMP_InputField _animalName;

    [SerializeField]
    private GameObject _roomPanel;

    private void Awake()
    {
        SaveManager.GetRoom();
        _roomPanel.SetActive(RoomManager.CurrentRoomId != string.Empty);
    }

    public async void CreateRoom()
    {
        var other = new Dictionary<string, object>
            {
                { "playerId", _otherID.text}
            };

        RoomManager.OtherPlayer = _otherID.text;

        var empty = await CloudCodeService.Instance.CallEndpointAsync<object>("RoomChecker", other);

        if (!(bool)empty) return;

        if (_animalName.text == string.Empty || _otherID.text == string.Empty)
        {
            Debug.LogWarning("Values aren't all assigned !");
            return;
        }

        RoomManager.CurrentRoomData = new RoomData
        {
            AnimalName = _animalName.text,
            ParentNote = "",
            AnimalStates = new List<AnimalState>
            {
                new AnimalState { Level = AnimalLevel.HUNGER, Value = 100f },
                new AnimalState { Level = AnimalLevel.SLEEP, Value = 100f },
                new AnimalState { Level = AnimalLevel.HYGIENE, Value = 100f },
                new AnimalState { Level = AnimalLevel.FUN, Value = 100f }
            },
        };

        RoomManager.UpdateRoom
        (
            System.Guid.NewGuid().ToString(),
            JsonUtility.ToJson(RoomManager.CurrentRoomData),
            _otherID.text,
            _animalName.text,
            Unity.Services.Authentication.AuthenticationService.Instance.PlayerId,
            JsonUtility.ToJson(ActiveDaysHandler.OtherDays),
            JsonUtility.ToJson(ActiveDaysHandler.ActiveDays)
        );
    }
}

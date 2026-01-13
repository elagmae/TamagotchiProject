using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RoomCreationHandler : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField _otherID;

    [SerializeField]
    private TMP_InputField _animalName;

    [SerializeField]
    private GameObject _roomPanel;

    [SerializeField]
    private ActiveDaysHandler _days;

    private void Awake()
    {
        SaveManager.GetRoom();
        _roomPanel.SetActive(RoomUpdater.CurrentRoomId != string.Empty);
    }

    public void CreateRoom()
    {
        if(_animalName.text == string.Empty || _otherID.text == string.Empty)
        {
            Debug.LogWarning("Values aren't all assigned !");
            return;
        }

        RoomUpdater.CurrentRoomData = new RoomData
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
            LastConnection = DateTime.Now,
        };

        RoomUpdater.UpdateRoom
        (
            System.Guid.NewGuid().ToString(),
            JsonUtility.ToJson(RoomUpdater.CurrentRoomData),
            _otherID.text,
            _animalName.text,
            Unity.Services.Authentication.AuthenticationService.Instance.PlayerId,
            JsonUtility.ToJson(_days.GetDays()[1]),
            JsonUtility.ToJson(_days.GetDays()[0])
        );
    }
}

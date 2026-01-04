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

    private void Awake()
    {
        SaveManager.GetRoom();
        _roomPanel.SetActive(RoomManager.CurrentRoomId != string.Empty);
    }

    public void CreateRoom()
    {
        if(_animalName.text == string.Empty || _otherID.text == string.Empty)
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
                new AnimalState { Level = "HUNGER", Value = 100f },
                new AnimalState { Level = "SLEEP", Value = 100f },
                new AnimalState { Level = "HYGIENE", Value = 100f },
                new AnimalState { Level = "FUN", Value = 100f }
            },
        };

        RoomManager.UpdateRoom
        (
            System.Guid.NewGuid().ToString(),
            JsonUtility.ToJson(RoomManager.CurrentRoomData),
            _otherID.text,
            _animalName.text,
            Unity.Services.Authentication.AuthenticationService.Instance.PlayerId
        );

        print(JsonUtility.ToJson(RoomManager.CurrentRoomData));

        // Assign Active Days (Player Data) => cf. Cloud Save.
    }
}

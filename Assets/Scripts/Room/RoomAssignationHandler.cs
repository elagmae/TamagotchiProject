using TMPro;
using UnityEngine;
using System.Collections.Generic;

public class RoomAssignationHandler : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _animalName;
    [SerializeField]
    private List<TextMeshProUGUI> _animalStatesTexts;
    [SerializeField]
    private GameObject _roomPanel;

    private void Awake()
    {
        SaveManager.OnRoomUpdated += AssignRoomInfos;
    }

    public void OpenRoom()
    {
        AssignRoomInfos(RoomManager.CurrentRoomId, RoomManager.CurrentRoomData);
    }

    public void AssignRoomInfos(string id, RoomData data)
    {
        _roomPanel.SetActive(true);
        _animalName.text = data.AnimalName;

        print(data);
        print(data.AnimalStates.Count);
        print(data.AnimalStates[0].Value);

        for(int i = 0; i < data.AnimalStates.Count; i++)
        {
            _animalStatesTexts[i].text = $"{data.AnimalStates[i].Level} : {data.AnimalStates[i].Value}%";
        }
    }
}

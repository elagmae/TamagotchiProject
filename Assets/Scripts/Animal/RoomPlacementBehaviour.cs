using System.Collections.Generic;
using UnityEngine;

public class RoomPlacementBehaviour : MonoBehaviour
{
    [SerializeField]
    private RectTransform _roomPanel;
    private List<RectTransform> _rooms = new();

    private void Awake()
    {
        for (int i = 0; i < _roomPanel.childCount; i++)
        {
            _rooms.Add(_roomPanel.GetChild(i).transform as RectTransform);
            PlaceRooms(i);
        }
    }

    public void PlaceRooms(int i)
    {
        _rooms[i].SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, _rooms[i].rect.width);
        _rooms[i].anchoredPosition = new Vector2(_rooms[i].rect.width * i, _rooms[i].anchoredPosition.y);
    }
}

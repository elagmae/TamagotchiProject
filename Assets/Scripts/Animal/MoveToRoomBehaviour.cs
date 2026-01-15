using UnityEngine;
using UnityEngine.UI;

public class MoveToRoomBehaviour : MonoBehaviour
{
    [SerializeField]
    private Button _leftButton;

    [SerializeField]
    private Button _rightButton;

    [SerializeField]
    private RectTransform _rooms;

    private int _currentRoomIndex;

    private void Awake()
    {
        _currentRoomIndex = 3;

        _leftButton.onClick.AddListener(() => MoveToNextRoom(-1));
        _rightButton.onClick.AddListener(() => MoveToNextRoom(1));
    }

    public void MoveToNextRoom(int direction)
    {
        if (_currentRoomIndex + direction >= 0 || _currentRoomIndex + direction <= StateManager.Instance.StateFills.Count - 1)
        {
            _currentRoomIndex += direction;
        }

        if (_currentRoomIndex + direction < -1 && direction == -1) _currentRoomIndex = StateManager.Instance.StateFills.Count - 1;
        if (_currentRoomIndex + direction > StateManager.Instance.StateFills.Count && direction == 1) _currentRoomIndex = 0;

        _rooms.anchoredPosition = new Vector2(-_currentRoomIndex * _rooms.rect.width, _rooms.anchoredPosition.y);
    }
}

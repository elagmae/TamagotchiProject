using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SleepHandler : MonoBehaviour
{
    [SerializeField]
    private Image _sleepState;
    private bool _isSleeping;
    private float _speed = 0.01f;

    private void Update()
    {
        if (!RoomUpdater.CurrentRoomData.AnimalStates[1].IsUnityNull())
        {
            if (_isSleeping && _sleepState.fillAmount < 1f) _sleepState.fillAmount += Time.deltaTime * _speed;
            else if (!_isSleeping && _sleepState.fillAmount > 0f) _sleepState.fillAmount -= Time.deltaTime * _speed;
        }
    }

    public void SetSleep(bool isSleeping)
    {
        _isSleeping = isSleeping;
    }
}

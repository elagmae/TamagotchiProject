using UnityEngine;

public class SleepBehaviour : MonoBehaviour
{
    private bool _isSleeping;
    private float _speed = 0.01f;

    private void Update()
    {
        if (StateManager.Instance != null)
        {
            if (_isSleeping && StateManager.Instance.StateFills[AnimalLevel.SLEEP].fillAmount < 1f) StateManager.Instance.AddToState(AnimalLevel.SLEEP, Time.deltaTime * _speed);

            else if (!_isSleeping && StateManager.Instance.StateFills[AnimalLevel.SLEEP].fillAmount > 0f) StateManager.Instance.RemoveFromState(AnimalLevel.SLEEP, Time.deltaTime * _speed);
        }
    }

    public void SetSleep(bool isSleeping)
    {
        _isSleeping = isSleeping;
    }
}

using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class SleepBehaviour : MonoBehaviour
{
    [SerializeField]
    private Toggle _sleepToggle;

    private void Start()
    {
        _sleepToggle.isOn = RoomManager.Instance.RoomData.IsAsleep;
    }

    private async void Update()
    {
        await Task.Delay(1000);

        if (StateManager.Instance != null)
        {
            if (RoomManager.Instance.RoomData.IsAsleep && StateManager.Instance.StateFills[AnimalLevel.SLEEP].fillAmount < 1f)
            {
                StateManager.Instance.AddToState(AnimalLevel.SLEEP, Time.deltaTime * StateManager.Instance.DecreasingSpeeds[AnimalLevel.SLEEP]);
            }

            else if (!RoomManager.Instance.RoomData.IsAsleep && StateManager.Instance.StateFills[AnimalLevel.SLEEP].fillAmount > 0f)
            {
                StateManager.Instance.RemoveFromState(AnimalLevel.SLEEP, Time.deltaTime * StateManager.Instance.DecreasingSpeeds[AnimalLevel.SLEEP]);
            }
        }
    }

    public void SetSleep(bool isSleeping)
    {
        RoomData data = RoomManager.Instance.RoomData;
        data.IsAsleep = isSleeping;
        RoomManager.Instance.RoomData = data;
    }
}

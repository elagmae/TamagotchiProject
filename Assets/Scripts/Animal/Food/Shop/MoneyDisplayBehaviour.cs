using TMPro;
using UnityEngine;

public class MoneyDisplayBehaviour : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _moneyDisplay;

    private void Start()
    {
        RoomManager.Instance.OnMoneyUpdate += UpdateMoneyDisplay;
        UpdateMoneyDisplay(RoomManager.Instance.Money);
    }

    private void UpdateMoneyDisplay(int money)
    {
        _moneyDisplay.text = money.ToString();
    }
}

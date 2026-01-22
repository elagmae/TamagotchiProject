using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FoodInventoryItemBehaviour : MonoBehaviour
{
    [field: SerializeField]
    public FoodData Data { get; set; }
    public TextMeshProUGUI AmountDisplay { get; private set; }
    public Image Image { get; private set; }

    private void Awake()
    {
        Image = GetComponent<Image>();
        AmountDisplay = GetComponentInChildren<TextMeshProUGUI>();
    }
}

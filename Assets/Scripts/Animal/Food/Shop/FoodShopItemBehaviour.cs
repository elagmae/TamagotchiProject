using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FoodShopItemBehaviour : MonoBehaviour
{
    [field:SerializeField]
    public FoodData FoodData { get; private set; }
    public Button Button { get;private set; }
    public Image Image { get;private set; }
    public TextMeshProUGUI PriceTmp { get; private set; } 

    private void Awake()
    {
        TryGetComponent<Button>(out Button button);
        Button = button;

        Image = gameObject.transform.GetChild(0).GetComponent<Image>();

        Image.sprite = StateManager.Instance.FoodInventoryHandler.FoodVisuals[FoodData];

        PriceTmp = GetComponentInChildren<TextMeshProUGUI>();
    }
}

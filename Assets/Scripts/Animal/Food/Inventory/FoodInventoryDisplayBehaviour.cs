using System.Linq;
using Unity.Services.CloudSave.Models;
using UnityEngine;

public class FoodInventoryDisplayBehaviour : MonoBehaviour
{
    [SerializeField]
    private GameObject _inventoryContainer;
    private FoodInventoryItemBehaviour[] _inventoryDisplay;

    private void Awake()
    {
        StateManager.Instance.FoodInventoryHandler.OnInventoryUpdated += UpdateInventoryDisplay;
       _inventoryDisplay = _inventoryContainer.GetComponentsInChildren<FoodInventoryItemBehaviour>(true);
    }

    private void UpdateInventoryDisplay(FoodData data, int amount)
    {
        foreach(FoodInventoryItemBehaviour item in _inventoryDisplay)
        {
            if(item.Data.Name == data.Name)
            {
                if(amount <= 0)
                {
                    item.gameObject.SetActive(false);
                }

                else
                {
                    item.gameObject.SetActive(true);
                    item.Image.sprite = StateManager.Instance.FoodInventoryHandler.FoodVisuals[data];
                    item.AmountDisplay.text = amount.ToString();
                }

                break;
            }
        }
    }
}

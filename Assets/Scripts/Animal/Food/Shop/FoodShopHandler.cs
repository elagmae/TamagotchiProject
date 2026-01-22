using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FoodShopHandler : MonoBehaviour
{
    [SerializeField]
    private GameObject _itemGrid;
    private List<FoodShopItemBehaviour> _shopItems;

    private void Start()
    {
        _shopItems = _itemGrid.GetComponentsInChildren<FoodShopItemBehaviour>(true).ToList();

        foreach (FoodShopItemBehaviour item in _shopItems)
        {
            //item.Image.sprite = StateManager.Instance.InventoryHandler.FoodSprites[item.FoodData.name];
            item.PriceTmp.text = item.FoodData.Price.ToString();

            item.Button.onClick.AddListener(() => BuyItem(item));
        }
    }

    public void BuyItem(FoodShopItemBehaviour item)
    {
        if(RoomManager.Instance.Money < item.FoodData.Price)
        {
            return;
        }

        RoomManager.Instance.ChangeMoneyAmount(RoomManager.Instance.Money - item.FoodData.Price);

        StateManager.Instance.FoodInventoryHandler.AddToInventory(item.FoodData);
    }
}

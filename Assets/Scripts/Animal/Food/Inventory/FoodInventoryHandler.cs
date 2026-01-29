using AYellowpaper.SerializedCollections;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.CloudSave;
using UnityEngine;

public class FoodInventoryHandler : MonoBehaviour
{
    public event Action<FoodData, int> OnInventoryUpdated;

    [field: SerializeField]
    public SerializedDictionary<FoodData, Sprite> FoodVisuals { get; set; } = new();

    public static Dictionary<string, int> Inventory { get; private set; } = new();

    private Dictionary<string, FoodData> _foods = new();

    private async void Awake()
    {
        var allDatas = Resources.LoadAll<FoodData>("Food/");

        foreach(var data in allDatas)
        {
            _foods[data.Name] = data;
        }

        try
        {
            Dictionary<string, Unity.Services.CloudSave.Models.Item> playerData = await CloudSaveService.Instance.Data.Player.LoadAsync(new HashSet<string> { "Inventory" });

            if (!playerData.ContainsKey("Inventory"))
            {
                if (Unity.Services.Authentication.AuthenticationService.Instance.IsSignedIn)
                {
                    await CloudSaveService.Instance.Data.Player.SaveAsync
                    (
                        new Dictionary<string, object>
                        {
                        { "Inventory", Inventory }
                        }
                    );
                }
            }

            Inventory = playerData["Inventory"].Value.GetAs<Dictionary<string, int>>();

            foreach (string name in Inventory.Keys) for (int i = 0; i < Inventory[name]; i++) OnInventoryUpdated?.Invoke(_foods[name], Inventory[name]);
        }

        catch (Exception e)
        {
            Debug.LogWarning("No inventory found, starting fresh.");
            Debug.Log(e.Message);
        }
    }

    public void AddToInventory(FoodData data)
    {
        if(Inventory.ContainsKey(data.Name)) Inventory[data.Name]++;
        else Inventory[data.Name] = 1;

        OnInventoryUpdated?.Invoke(data, Inventory[data.Name]);
    }

    public void RemoveFromInventory(FoodData data)
    {
        if(Inventory.ContainsKey(data.Name))
        {
            Inventory[data.Name]--;
            OnInventoryUpdated?.Invoke(data, Inventory[data.Name]);

            if (Inventory[data.Name] <= 0) Inventory.Remove(data.Name);
        }
    }

    public static async Task SaveInventory()
    {
        try
        {
            if (Unity.Services.Authentication.AuthenticationService.Instance.IsSignedIn)
            {
                await CloudSaveService.Instance.Data.Player.SaveAsync
                (
                    new Dictionary<string, object>
                    {
                        { "Inventory", Inventory }
                    }
                );
            }
        }

        catch (Exception e)
        {
            Debug.LogWarning("Could not save inventory on quit: " + e.Message);
        }
    }
}

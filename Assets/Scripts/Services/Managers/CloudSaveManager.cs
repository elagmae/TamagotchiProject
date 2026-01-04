using System.Collections.Generic;
using System.IO;
using Unity.Services.CloudSave;
using Unity.Services.CloudSave.Models;
using UnityEngine;

public static class CloudSaveManager
{
    public static async void LoadCustomItemData(string key) // For Cloud Save Custom Items (not Player Data). => cf. Cloud Code.
    {
        var customItemId = key;
        var customItemData = await CloudSaveService.Instance.Data.Custom.LoadAllAsync(customItemId);

        var jsonObj = customItemData[key].Value;

        string json = JsonUtility.ToJson(jsonObj);
        Debug.Log($"JSON: {json}");
    }

    #region Player Data

    public static async void SaveData(string key, object value)
    {
        var playerData = new Dictionary<string, object>{ { key, value } };

        await CloudSaveService.Instance.Data.Player.SaveAsync(playerData);
        Debug.Log($"Saved data {string.Join(',', playerData)}");
    }

    public static async void LoadData(string key)
    {
        var playerData = await CloudSaveService.Instance.Data.Player.LoadAsync(new HashSet<string> { key });

        if (playerData.TryGetValue(key, out var savedKey)) Debug.Log($"value: {savedKey.Value.GetAs<string>()}");
        RoomManager.CurrentRoomId = savedKey.Value.GetAs<string>();
    }

    public static async void DeleteData(string key)
    {
        var options = new Unity.Services.CloudSave.Models.Data.Player.DeleteOptions();
        await CloudSaveService.Instance.Data.Player.DeleteAsync(key, options);
    }

    public static async void GetAllSavedKeys()
    {
        var keys = await CloudSaveService.Instance.Data.Player.ListAllKeysAsync();
        for (int i = 0; i < keys.Count; i++)
        {
            Debug.Log(keys[i].Key);
        }
    }

    #endregion

    #region PlayerFiles

    public static async void SavePlayerFileBytes(string fileName, string key)
    {
        byte[] fileBytes = System.IO.File.ReadAllBytes(fileName);
        await CloudSaveService.Instance.Files.Player.SaveAsync(key, fileBytes);
    }

    public static async void SavePlayerFileStream(string fileName, string key)
    {
        Stream fileStream = System.IO.File.OpenRead(fileName);
        await CloudSaveService.Instance.Files.Player.SaveAsync(key, fileStream);
    }

    public static async void GetPlayerFileAsByteArray(string key)
    {
        byte[] file = await CloudSaveService.Instance.Files.Player.LoadBytesAsync(key);
    }

    public static async void GetPlayerFileAsStream(string key)
    {
        Stream file = await CloudSaveService.Instance.Files.Player.LoadStreamAsync(key);
    }

    public static async void DeletePlayerFile(string key)
    {
        await CloudSaveService.Instance.Files.Player.DeleteAsync(key);
    }

    public static async void GetAllSavedFiles()
    {
        List<FileItem> files = await CloudSaveService.Instance.Files.Player.ListAllAsync();
        for (int i = 0; i < files.Count; i++)
        {
            Debug.Log(files[i]);
        }
    }

    public static async void GetPlayerFileMetadata()
    {
        var metadata = await CloudSaveService.Instance.Files.Player.GetMetadataAsync("fileName");
        Debug.Log(metadata.Key);
        Debug.Log(metadata.Size);
        Debug.Log(metadata.ContentType);
        Debug.Log(metadata.Created);
        Debug.Log(metadata.Modified);
        Debug.Log(metadata.WriteLock);
    }

    #endregion
}

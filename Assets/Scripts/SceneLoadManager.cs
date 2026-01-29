using System;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadManager : MonoBehaviour
{
    public static SceneLoadManager Instance;

    public event Action<string> OnSceneLoaded;
    public event Action<string> OnSceneUnloaded;

    [field:SerializeField]
    public RectTransform UserPanel {get;set;}

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public async Task LoadScene(string sceneName)
    {
        if (sceneName == "Exit") Exit();

        await Task.Yield();

        //add transitions here.
        OnSceneUnloaded?.Invoke(SceneManager.GetActiveScene().name);
        await SceneManager.LoadSceneAsync(sceneName);
        OnSceneLoaded?.Invoke(sceneName);

        UserPanel.gameObject.SetActive(sceneName != "Connection" && sceneName != "Menu");
    }

    public async Task SaveGame()
    {
        await RoomManager.Instance.UpdateRoom();
        await RoomManager.Instance.SaveMoney();
        await FoodInventoryHandler.SaveInventory();
    }

    public async void Exit()
    {
        await Task.Yield();
        await SaveGame();

#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#endif
        Application.Quit();
    }

    private async void OnApplicationQuit()
    {
        await SaveGame();
    }
}

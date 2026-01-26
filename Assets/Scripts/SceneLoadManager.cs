using System;
using System.Threading.Tasks;
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
        await Task.Yield();

        //add transitions here.
        OnSceneUnloaded?.Invoke(SceneManager.GetActiveScene().name);
        await SceneManager.LoadSceneAsync(sceneName);
        OnSceneLoaded?.Invoke(sceneName);

        UserPanel.gameObject.SetActive(sceneName != "Connection" && sceneName != "Menu");
    }
}

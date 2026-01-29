using AYellowpaper.SerializedCollections;
using UnityEngine;
using UnityEngine.UI;

public class ClickListenerBehaviour : MonoBehaviour
{
    [SerializeField]
    private SerializedDictionary<Button, string> _sceneButtons = new();

    private void Awake()
    {
        foreach (Button button in _sceneButtons.Keys)
        {
            button.onClick.AddListener(() => ChangeScene(button));
        }
    }

    public async void ChangeScene(Button button)
    {
        await SceneLoadManager.Instance.LoadScene(_sceneButtons[button]);
    }
}

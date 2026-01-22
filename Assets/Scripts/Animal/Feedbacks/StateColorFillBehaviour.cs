using UnityEngine;

public class StateColorFillBehaviour : MonoBehaviour
{
    [SerializeField]
    private Canvas _loadingCanvas;

    private void Awake()
    {
        StateManager.Instance.OnFillUpdated += ColorFill;
    }

    private void ColorFill(AnimalLevel level, float value)
    {
        switch (value)
        {
            case float v when (v <= 1f && v >= 0.7f):
                StateManager.Instance.StateFills[level].Fill.color = Color.green;
                break;

            case float v when (v < 0.7f && v >= 0.35f):
                StateManager.Instance.StateFills[level].Fill.color = Color.yellow;
                break;
                
            case float v when (v < 0.35f && v >= 0f):
                StateManager.Instance.StateFills[level].Fill.color = Color.red;
                break;
        }

        if(_loadingCanvas.gameObject.activeInHierarchy) _loadingCanvas.gameObject.SetActive(false);
    }
}

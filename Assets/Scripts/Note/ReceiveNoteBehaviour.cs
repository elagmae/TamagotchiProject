using TMPro;
using UnityEngine;

public class ReceiveNoteBehaviour : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _noteDisplay;
    [SerializeField]
    private RectTransform _noteContainer;

    private ParentNoteSaveBehaviour _noteSaver;

    private void Awake()
    {
        TryGetComponent(out _noteSaver);
        _noteSaver.OnNote += ShowNote;
    }

    private void ShowNote(string note)
    {
        print("note = " + note);
        _noteContainer.gameObject.SetActive(true);
        _noteDisplay.text = note;
    }
}

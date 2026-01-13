using TMPro;
using UnityEngine;

public class ProfileInfosSetter : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _id;

    public static string Clipboard
    {
        get { return GUIUtility.systemCopyBuffer; }
        set { GUIUtility.systemCopyBuffer = value; }
    }

    private void Awake()
    {
        _id.text = AuthenticationManager.UserId;
    }

    public void CopyId()
    {
        Clipboard = _id.text;
    }
}

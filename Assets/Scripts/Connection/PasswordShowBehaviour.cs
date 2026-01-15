using TMPro;
using UnityEngine;

public class PasswordShowBehaviour : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField _password;

    public void ShowPassword(bool state)
    {
        TMP_InputField.ContentType type = state ? TMP_InputField.ContentType.Standard : TMP_InputField.ContentType.Password;
        _password.contentType = type;
        _password.ForceLabelUpdate();
    }
}

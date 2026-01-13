using UnityEngine;
using UnityEngine.UI;

public class PlayerRememberBehaviour : MonoBehaviour
{
    [SerializeField]
    private Toggle _remember;

    private AuthenticationBehaviour _auth;

    private bool _canRemember;

    private void Awake()
    {
        TryGetComponent(out _auth);
        if (PlayerPrefs.HasKey("id") && PlayerPrefs.HasKey("password"))
        {
            _auth.Id.text = PlayerPrefs.GetString("id");
            _auth.Password.text = PlayerPrefs.GetString("password");
            _remember.isOn = true;
        }
    }

    private void RememberAccountData()
    {
        PlayerPrefs.SetString("id", _auth.Id.text.Trim());
        PlayerPrefs.SetString("password", _auth.Password.text.Trim());

        PlayerPrefs.Save();
    }

    public void CanRemember(bool state)
    {
        _canRemember = state;

        if (_canRemember) RememberAccountData();

        else
        {
            PlayerPrefs.DeleteKey("id");
            PlayerPrefs.DeleteKey("password");
        }
    }
}

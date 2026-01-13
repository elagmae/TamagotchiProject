using System;
using TMPro;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;

public class AuthenticationBehaviour : MonoBehaviour
{
    public event Action OnPlayerConnected;

    [field:SerializeField]
    public TMP_InputField Id { get;private set; }

    [field:SerializeField]
    public TMP_InputField Password { get; private set; }

    [SerializeField]
    private TextMeshProUGUI _errorTmp;

    private void Awake()
    {
        UnityServices.InitializeAsync();

        _errorTmp.transform.parent.transform.parent.gameObject.SetActive(false);
    }

    public async void SignIn()
    {
        _errorTmp.transform.parent.transform.parent.gameObject.SetActive(false);

        try
        {
            await AuthenticationService.Instance.SignInWithUsernamePasswordAsync(Id.text.Trim(), Password.text.Trim());
            OnPlayerConnected?.Invoke(); // If player already has a room, he can skip creation scene.
        }

        catch (AuthenticationException ex)
        {
            _errorTmp.text = ex.Message;
            _errorTmp.transform.parent.transform.parent.gameObject.SetActive(true);
        }

        catch (RequestFailedException ex)
        {
            _errorTmp.text = ex.Message;
            _errorTmp.transform.parent.transform.parent.gameObject.SetActive(true);
        }
    }

    public async void SignUp()
    {
        _errorTmp.transform.parent.transform.parent.gameObject.SetActive(false);

        try
        {
            await AuthenticationService.Instance.SignUpWithUsernamePasswordAsync(Id.text.Trim(), Password.text.Trim());
            OnPlayerConnected?.Invoke(); // If player already has a room, he can skip creation scene.
        }

        catch (AuthenticationException ex)
        {
            _errorTmp.text = ex.Message;
            _errorTmp.transform.parent.transform.parent.gameObject.SetActive(true);
        }

        catch (RequestFailedException ex)
        {
            _errorTmp.text = ex.Message;
            _errorTmp.transform.parent.transform.parent.gameObject.SetActive(true);
        }
    }
}

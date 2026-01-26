using System;
using System.Threading.Tasks;
using TMPro;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;

public class PasswordUpdateBehaviour : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _errorTmp;
    [SerializeField] 
    private TMP_InputField _currentPassword;
    [SerializeField]
    private TMP_InputField _newPassword;

    [SerializeField]
    private GameObject _successPanel;

    public async void UpdatePassword()
    {
        _errorTmp.transform.parent.gameObject.SetActive(false);
        _successPanel.SetActive(false);

        try
        {
            await AuthenticationService.Instance.UpdatePasswordAsync(_currentPassword.text.Trim(), _newPassword.text.Trim());
            _successPanel.SetActive(true);
            await Task.Delay(3000);
            _successPanel.SetActive(false);
            _errorTmp.transform.parent.gameObject.SetActive(false);
        }

        catch (AuthenticationException ex)
        {
            Debug.LogException(ex);
            _errorTmp.text = ex.Message;
            _errorTmp.transform.parent.gameObject.SetActive(true);
        }

        catch (RequestFailedException ex)
        {
            Debug.LogException(ex);
            _errorTmp.transform.parent.gameObject.SetActive(true);
            _errorTmp.text = ex.Message;
        }

        catch (Exception e)
        {
            Debug.LogException(e);
            _errorTmp.text = e.Message;
            _errorTmp.transform.parent.gameObject.SetActive(true);
        }
    }
}

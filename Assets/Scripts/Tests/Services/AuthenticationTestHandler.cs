using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AuthenticationTestHandler : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField _id;
    [SerializeField]
    private TMP_InputField _mdp;

    private void Awake()
    {
        Unity.Services.Core.UnityServices.InitializeAsync();
    }

    public async void SignUp()
    {
       await AuthenticationManager.SignUpWithUsernamePasswordAsync(_id.text.Trim(), _mdp.text.Trim());
       AuthenticationManager.UserId = Unity.Services.Authentication.AuthenticationService.Instance.PlayerId;

       if (Unity.Services.Authentication.AuthenticationService.Instance.IsSignedIn) SceneManager.LoadScene("Hub");
    }

    public async void SignIn()
    {
        await AuthenticationManager.SignInWithUsernamePasswordAsync(_id.text.Trim(), _mdp.text.Trim());
        AuthenticationManager.UserId = Unity.Services.Authentication.AuthenticationService.Instance.PlayerId;

        if(Unity.Services.Authentication.AuthenticationService.Instance.IsSignedIn) SceneManager.LoadScene("Hub");
    }
}

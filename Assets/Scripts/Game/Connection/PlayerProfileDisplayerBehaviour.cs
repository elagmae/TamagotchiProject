using TMPro;
using UnityEngine;

public class PlayerProfileDisplayerBehaviour : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _playerId;

    private void OnEnable()
    {
        _playerId.text = Unity.Services.Authentication.AuthenticationService.Instance.PlayerId;
    }
}

using UnityEngine;
using UnityEngine.UI;

public class ShopInteractableVerification : MonoBehaviour
{
    private Button _btn;

    private void Awake() => TryGetComponent(out _btn);

    public void CanInteract()
    {
        _btn.interactable = !RoomManager.Instance.RoomData.IsAsleep;
    }
}

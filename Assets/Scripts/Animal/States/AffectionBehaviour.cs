using UnityEngine;
using UnityEngine.EventSystems;

public class AffectionBehaviour : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        if (RoomManager.Instance.RoomData.IsAsleep) return;
        StateManager.Instance.AddToState(AnimalLevel.FUN, 0.05f);
    }
}

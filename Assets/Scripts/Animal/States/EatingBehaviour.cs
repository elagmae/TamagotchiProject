using UnityEngine;
using UnityEngine.EventSystems;

public class EatingBehaviour : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private FoodInventoryItemBehaviour _item;
    private Vector2 _defaultPos;

    private void Awake()
    {
        TryGetComponent<FoodInventoryItemBehaviour>(out _item);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {

        _defaultPos = transform.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (RoomManager.Instance.RoomData.IsAsleep) return;

        bool canFeed = RectTransformUtility.RectangleContainsScreenPoint(StateManager.Instance.Animal.rectTransform, eventData.pointerCurrentRaycast.screenPosition, Camera.main);

        if (canFeed)
        {
            StateManager.Instance.FoodInventoryHandler.RemoveFromInventory(_item.Data);
            StateManager.Instance.AddToState(AnimalLevel.HUNGER, _item.Data.FoodIncrease);
        }

        transform.position = _defaultPos;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (RoomManager.Instance.RoomData.IsAsleep) return;
        transform.position = eventData.pointerCurrentRaycast.worldPosition;
    }
}

using UnityEngine;
using UnityEngine.EventSystems;

public class ShowerBehaviour : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField]
    private float _cleanseThreshold = 15f;
    [SerializeField, Range(0f, 1f)]
    private float _cleanseAmount = 0.01f;
    private Vector2 _defaultPos;
    private Vector2 _previousPos;

    public void OnBeginDrag(PointerEventData eventData)
    {
        _defaultPos = transform.position;
        print("touched");
    }

    public void OnDrag(PointerEventData eventData)
    {
        print("drag");
        if (RoomManager.Instance.RoomData.IsAsleep) return;
        bool isShowering = RectTransformUtility.RectangleContainsScreenPoint(StateManager.Instance.Animal.rectTransform, eventData.position);

        _previousPos = transform.position;
        transform.position = eventData.position;

        Vector2 velocity = (Vector2)transform.position - _previousPos;

        if(velocity.magnitude >= _cleanseThreshold && isShowering) StateManager.Instance.AddToState(AnimalLevel.HYGIENE, _cleanseAmount);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        print("stop");
        transform.position = _defaultPos;
    }
}

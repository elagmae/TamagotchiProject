using UnityEngine;
using UnityEngine.EventSystems;

public class ShowerBehaviour : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField]
    private float _cleanseThreshold = 15f;
    [SerializeField, Range(0f, 1f)]
    private float _cleanseAmount = 0.01f;
    [SerializeField]
    private ParticleSystem _showerParticles;
    private Vector2 _defaultPos;
    private Vector2 _previousPos;

    public void OnBeginDrag(PointerEventData eventData)
    {
        _defaultPos = transform.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (RoomManager.Instance.RoomData.IsAsleep) return;
        bool isShowering = RectTransformUtility.RectangleContainsScreenPoint(StateManager.Instance.Animal.rectTransform, eventData.pointerCurrentRaycast.screenPosition, Camera.main);

        _previousPos = transform.position;
        transform.position = eventData.pointerCurrentRaycast.worldPosition;

        Vector2 velocity = (Vector2)transform.position - _previousPos;

        if(velocity.magnitude >= _cleanseThreshold && isShowering)
        {
            StateManager.Instance.AddToState(AnimalLevel.HYGIENE, _cleanseAmount);
            if(!_showerParticles.isPlaying) _showerParticles.Play();
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.position = _defaultPos;
    }
}

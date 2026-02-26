using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HoverInteractionFeedback : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Material _hoverMaterial;
    private Image _image;

    public void OnPointerEnter(PointerEventData eventData)
    {
        _image.material = _hoverMaterial;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _image.material = null;
    }

    private void Awake()
    {
         TryGetComponent(out _image);
        //_image.alphaHitTestMinimumThreshold = 1f;
    }
}

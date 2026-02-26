using UnityEngine;
using UnityEngine.EventSystems;

public class AffectionBehaviour : MonoBehaviour, IPointerClickHandler
{
    [SerializeField, Range(0f, 1f)]
    private float _affectionIncreaseAmount = 0.05f;
    [SerializeField]
    private ParticleSystem _affectionParticles;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (RoomManager.Instance.RoomData.IsAsleep) return;
        StateManager.Instance.AddToState(AnimalLevel.FUN, _affectionIncreaseAmount);
        if(!_affectionParticles.isPlaying)_affectionParticles.Play();
    }
}

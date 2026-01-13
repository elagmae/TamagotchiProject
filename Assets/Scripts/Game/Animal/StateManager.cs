using AYellowpaper.SerializedCollections;
using UnityEngine;
using UnityEngine.UI;

public class StateManager : MonoBehaviour
{
    public static StateManager Instance;

    [field:SerializeField]
    public SerializedDictionary<AnimalLevel, Image> StateFills { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        foreach(AnimalLevel level in StateFills.Keys)
        {
            StateFills[level].fillAmount = RoomManager.Instance.RoomData.AnimalStates[(int)level].Value;
        }
    }

    public void AddToState(AnimalLevel level, float amount)
    {
        AnimalState state = RoomManager.Instance.RoomData.AnimalStates[(int)level];
        state.Value += amount;

        StateFills[level].fillAmount = state.Value;
        RoomManager.Instance.RoomData.AnimalStates[(int)level] = state;
    }

    public void RemoveFromState(AnimalLevel level, float amount)
    {
        AnimalState state = RoomManager.Instance.RoomData.AnimalStates[(int)level];
        state.Value -= amount;

        StateFills[level].fillAmount = state.Value;
        RoomManager.Instance.RoomData.AnimalStates[(int)level] = state;
    }
}

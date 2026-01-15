using AYellowpaper.SerializedCollections;
using System;
using UnityEngine;
using UnityEngine.UI;

public class StateManager : MonoBehaviour
{
    public static StateManager Instance;

    public event Action<AnimalLevel, float> OnFillUpdated;

    private StatesBeforeLoadBehaviour _statesBeforeLoad;

    [field:SerializeField]
    public SerializedDictionary<AnimalLevel, State> StateFills { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        TryGetComponent(out _statesBeforeLoad);

        foreach (AnimalLevel level in StateFills.Keys)
        {
            StateFills[level].Fill.fillAmount = RoomManager.Instance.RoomData.AnimalStates[(int)level].Value;
            _statesBeforeLoad.CalculateAwayFills(level);
        }
    }

    public void AddToState(AnimalLevel level, float amount)
    {
        AnimalState state = RoomManager.Instance.RoomData.AnimalStates[(int)level];
        state.Value += amount;

        if (state.Value > 1f) state.Value = 1f;

        StateFills[level].Fill.fillAmount = state.Value;
        OnFillUpdated?.Invoke(level, state.Value);

        RoomManager.Instance.RoomData.AnimalStates[(int)level] = state;
    }

    public void RemoveFromState(AnimalLevel level, float amount)
    {
        AnimalState state = RoomManager.Instance.RoomData.AnimalStates[(int)level];
        state.Value -= amount;

        if (state.Value < 0f) state.Value = 0f;

        StateFills[level].Fill.fillAmount = state.Value;
        OnFillUpdated?.Invoke(level, state.Value);

        RoomManager.Instance.RoomData.AnimalStates[(int)level] = state;
    }
}

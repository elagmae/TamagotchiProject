using AYellowpaper.SerializedCollections;
using System;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StateManager : MonoBehaviour
{
    #region Initialization

    public static StateManager Instance;

    public event Action<AnimalLevel, float> OnFillUpdated;

    [field:SerializeField]
    public SerializedDictionary<AnimalLevel, State> StateFills { get; private set; }

    [field : SerializeField]
    public FoodInventoryHandler FoodInventoryHandler { get; private set; }

    [field: SerializeField]
    public Image Animal { get;private set; }

    [SerializeField]
    private TextMeshProUGUI _animalName;

    private StatesBeforeLoadBehaviour _statesBeforeLoad;

    private void Awake()
    {
        #region Singleton Pattern

        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        #endregion

        TryGetComponent(out _statesBeforeLoad);

        if (RoomManager.Instance == null) return;

        #region Initialize States

        foreach (AnimalLevel level in StateFills.Keys)
        {
            StateFills[level].Fill.fillAmount = RoomManager.Instance.RoomData.AnimalStates[(int)level].Value;
            _statesBeforeLoad.CalculateAwayFills(level);

            OnFillUpdated?.Invoke(level, RoomManager.Instance.RoomData.AnimalStates[(int)level].Value);
        }

        #endregion

        _animalName.text = RoomManager.Instance.RoomData.AnimalName;

        if (RoomManager.Instance.RoomData.LastConnection.Year < 2000) return;
        double timeAway = (DateTime.Now - RoomManager.Instance.RoomData.LastConnection).TotalSeconds;

        if ((int)timeAway < 0) return;
        RoomManager.Instance.ChangeMoneyAmount(RoomManager.Instance.Money + (int)(timeAway / 60f));
    }

    #endregion

    private async void Update()
    {
        foreach (AnimalLevel level in StateFills.Keys)
        {
            await Task.Delay(1000);

            if (Instance != null && StateFills != null && StateFills[level].Fill.fillAmount > 0f && level != AnimalLevel.SLEEP)
            {
                RemoveFromState(level, Time.deltaTime * StateFills[level].DecreasingSpeed / 3600f);
            }
        }
    }

    public void AddToState(AnimalLevel level, float amount)
    {
        AnimalState state = RoomManager.Instance.RoomData.AnimalStates[(int)level];
        state.Value += amount;

        if (state.Value >= 1f) state.Value = 1f;

        StateFills[level].Fill.fillAmount = state.Value;
        RoomManager.Instance.RoomData.AnimalStates[(int)level] = state;

        OnFillUpdated?.Invoke(level, state.Value);
    }

    public void RemoveFromState(AnimalLevel level, float amount)
    {
        AnimalState state = RoomManager.Instance.RoomData.AnimalStates[(int)level];
        state.Value -= amount;

        if (state.Value <= 0f) state.Value = 0f;

        StateFills[level].Fill.fillAmount = state.Value;
        OnFillUpdated?.Invoke(level, state.Value);

        RoomManager.Instance.RoomData.AnimalStates[(int)level] = state;
    }

    private void OnDestroy()
    {
        OnFillUpdated = null;
    }
}

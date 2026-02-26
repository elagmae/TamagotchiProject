using System;
using UnityEngine;

public class StatesBeforeLoadBehaviour : MonoBehaviour
{
    public void CalculateAwayFills(AnimalLevel level)
    {
        var lastConnection = RoomManager.Instance.RoomData.LastConnection;

        if (lastConnection.Year < 2000) return;

        double timeAway = (DateTime.UtcNow - lastConnection).TotalSeconds;

        if (timeAway <= 0) return;

        float amount = ((float)timeAway / 3600f) * StateManager.Instance.StateFills[level].DecreasingSpeed;

        print(amount);

        if (level == AnimalLevel.SLEEP && RoomManager.Instance.RoomData.IsAsleep) StateManager.Instance.AddToState(level, amount);
        else StateManager.Instance.RemoveFromState(level, amount);
    }
}

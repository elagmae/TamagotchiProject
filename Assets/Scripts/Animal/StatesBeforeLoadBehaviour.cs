using System;
using UnityEngine;

public class StatesBeforeLoadBehaviour : MonoBehaviour
{
    public void CalculateAwayFills(AnimalLevel level)
    {
        var lastConnection = RoomManager.Instance.RoomData.LastConnection;

        if (lastConnection.Year < 2000) return;

        double timeAway = (DateTime.Now - lastConnection).TotalSeconds;

        if (timeAway <= 0) return;

        timeAway = Math.Min(timeAway, 86400);

        float amount = (float)(timeAway / 60f) * StateManager.Instance.StateFills[level].DecreasingSpeed / 3600f;

        if (level == AnimalLevel.SLEEP && RoomManager.Instance.RoomData.IsAsleep) StateManager.Instance.AddToState(level, amount);
        else StateManager.Instance.RemoveFromState(level, amount);
    }
}

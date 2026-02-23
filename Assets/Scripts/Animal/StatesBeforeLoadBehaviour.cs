using System;
using UnityEngine;

public class StatesBeforeLoadBehaviour : MonoBehaviour
{
    public void CalculateAwayFills(AnimalLevel level)
    {
        if (RoomManager.Instance.RoomData.LastConnection.Year < 2000) return;

        double timeAway = (DateTime.Now - RoomManager.Instance.RoomData.LastConnection).TotalSeconds;
        print("timeAway: " + timeAway);

        if (level == AnimalLevel.SLEEP && RoomManager.Instance.RoomData.IsAsleep)
        {
            StateManager.Instance.AddToState(level, (float)timeAway * Time.deltaTime * StateManager.Instance.StateFills[level].DecreasingSpeed);
            print($"{level} increased by {(float)timeAway/10f * Time.deltaTime * StateManager.Instance.StateFills[level].DecreasingSpeed}");
        }

        else
        {
            StateManager.Instance.RemoveFromState(level, (float)timeAway * Time.deltaTime * StateManager.Instance.StateFills[level].DecreasingSpeed);
            print($"{level} decreased by {(float)timeAway/10f * Time.deltaTime * StateManager.Instance.StateFills[level].DecreasingSpeed}");
        }
    }
}

using System;
using UnityEngine;

public class StatesBeforeLoadBehaviour : MonoBehaviour
{
    public void CalculateAwayFills(AnimalLevel level)
    {
        double timeAway = (DateTime.Now - RoomManager.Instance.RoomData.LastConnection).TotalSeconds;

        if(level == AnimalLevel.SLEEP && RoomManager.Instance.RoomData.IsAsleep)
        {
            StateManager.Instance.AddToState(level, (float)timeAway * Time.deltaTime * StateManager.Instance.DecreasingSpeeds[level]);
        }

        StateManager.Instance.RemoveFromState(level, (float)timeAway * Time.deltaTime * StateManager.Instance.DecreasingSpeeds[level]);
    }
}

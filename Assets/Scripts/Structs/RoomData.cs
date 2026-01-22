using System;
using System.Collections.Generic;

[Serializable]
public struct RoomData
{
    public string AnimalName;
    public List<AnimalState> AnimalStates;
    public DateTime LastConnection;
    public bool IsAsleep;
    public string[] Parents;
}

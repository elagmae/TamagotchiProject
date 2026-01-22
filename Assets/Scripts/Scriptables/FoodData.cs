using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "Food", menuName = "Data/Food")]
public class FoodData : ScriptableObject
{
    public string Name = string.Empty;
    public int Price;
    public float FoodIncrease;
}

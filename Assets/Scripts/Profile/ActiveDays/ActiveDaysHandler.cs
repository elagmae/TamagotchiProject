using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class ActiveDaysHandler : MonoBehaviour
{
    [SerializeField]
    private List<Button> _allDays = new();

    [Serializable]
    public struct Days
    {
        public List<int> Day;
    }
    public static Days ActiveDays = new();
    public static Days OtherDays = new();

    private void Awake()
    {
        OtherDays.Day = new(){ 0, 1, 2, 3, 4, 5, 6 };
        ActiveDays.Day = new();

        foreach (var day in _allDays)
        {
            day.onClick.AddListener(() => OnDayClicked(day));
        }

    }

    public void OnDayClicked(Button day)
    {
        if(ActiveDays.Day.Contains(day.transform.GetSiblingIndex()))
        {
            day.targetGraphic.color = Color.white;
            ActiveDays.Day.Remove(day.transform.GetSiblingIndex());
            OtherDays.Day.Add(day.transform.GetSiblingIndex());
        }

        else
        {
            day.targetGraphic.color = Color.green;
            ActiveDays.Day.Add(day.transform.GetSiblingIndex());
            OtherDays.Day.Remove(day.transform.GetSiblingIndex());
        }
    }

    public void ConfirmDays()
    {
        ActiveDays.Day.Sort();
        OtherDays.Day.Sort();
    }
}

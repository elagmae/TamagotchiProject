using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class ActiveDaysHandler : MonoBehaviour
{
    [SerializeField]
    private List<Button> _allDays = new();

    private Days _activeDays;
    private Days _otherDays;

    private void Awake()
    {
        _otherDays.Day = new(){ 0, 1, 2, 3, 4, 5, 6 };
        _activeDays.Day = new();

        foreach (var day in _allDays)
        {
            day.onClick.AddListener(() => OnDayClicked(day));
        }
    }

    private void OnDayClicked(Button day)
    {
        if(_activeDays.Day.Contains(day.transform.GetSiblingIndex()))
        {
            day.targetGraphic.color = Color.white;
            _activeDays.Day.Remove(day.transform.GetSiblingIndex());
            _otherDays.Day.Add(day.transform.GetSiblingIndex());
        }

        else
        {
            day.targetGraphic.color = Color.green;
            _activeDays.Day.Add(day.transform.GetSiblingIndex());
            _otherDays.Day.Remove(day.transform.GetSiblingIndex());
        }
    }

    public void ConfirmDays()
    {
        _activeDays.Day.Sort();
        _otherDays.Day.Sort();
    }

    public Days[] GetDays()
    {
        return new[] { _activeDays, _otherDays };
    }
}

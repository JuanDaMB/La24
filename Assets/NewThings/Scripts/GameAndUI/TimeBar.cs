using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimeBar : MonoBehaviour
{
    [SerializeField] private Image _bar;
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private float maxTime;
    [SerializeField] private Gradient _gradient;
    private float _time;
    private bool TimeRunning = false;
    public Action OnEndTimer;

    private void SetPercentage(float value)
    {
        _bar.fillAmount = value;
        _bar.color = _gradient.Evaluate(value);
    }

    private void SetTime(float value)
    {
        _text.text = value.ToString("00");
    }

    public void SetMaxTime(float CurrentMaxTime)
    {
        maxTime = CurrentMaxTime;
    }
    public void StartTime()
    {
        if (TimeRunning)
        {
            return;
        }
        _time = maxTime;
        TimeRunning = true;
    }

    public void EndTime()
    {
        _time = 0f;
    }

    private void Update()
    {
        if (!TimeRunning)
        {
            return;
        }

        if (GlobalObjects.State == GameState.Bet)
        {
            _time -= Time.unscaledDeltaTime;   
        }
        if (_time >= 0f)
        {
            SetPercentage(_time/maxTime);   
            SetTime(_time);
        }
        else
        {
            OnEndTimer?.Invoke();
            SetPercentage(0f);
            SetTime(0f);
            TimeRunning = false;
        }
    }
}

using System;
using System.Globalization;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public TMP_Text TimeObject;
    public static Timer Instance;
    public delegate void GameSecond(TimeSpan time);
    public static event GameSecond OnGameSecond;
    public delegate void GameMinute(TimeSpan time);
    public static event GameMinute OnGameMinute;
    readonly private string TimeFormat = "mm\\:ss";
    private void Awake()
    {
        if (Instance == null)
        Instance = this;
    }
    void Start()
    {
        TimeObject.text = new DateTime().ToString(TimeFormat);
        InvokeRepeating("OnSecond", 0,1);
    }
    private void OnSecond()
    {
        TimeSpan currTime = TimeSpan.ParseExact(TimeObject.text, TimeFormat, CultureInfo.InvariantCulture).Add(new TimeSpan(0, 0, 1));
        TimeObject.text = currTime.ToString(TimeFormat);
        OnGameSecond?.Invoke(currTime);
        if (currTime.Seconds % 60 == 0) OnGameMinute?.Invoke(currTime);
    }
}

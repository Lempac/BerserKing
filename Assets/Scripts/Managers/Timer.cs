using System;
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
    public string TimeFormat = "mm\\:ss";
    public TimeSpan Time;
    private void Awake()
    {
        if (Instance != null) Debug.LogError("Only one Timer!");
        else Instance = this;
    }
    void Start()
    {
        if (TimeObject != null) TimeObject.text = new DateTime().ToString(TimeFormat);
        InvokeRepeating("OnSecond", 0,1);
    }
    private void OnSecond()
    {
        Time = Time.Add(new TimeSpan(0, 0, 1));
        if(TimeObject != null) TimeObject.text = Time.ToString(TimeFormat);
        OnGameSecond?.Invoke(Time);
        if (Time.Seconds % 60 == 0) OnGameMinute?.Invoke(Time);
    }
}

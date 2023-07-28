using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public TMP_Text TimeObject;
    public static Timer Instance;
    //public delegate void GameSecond(TimeSpan time);
    //public static event GameSecond OnGameSecond;
    //public UnityEvent<TimeSpan> OnGameSecond;
    //public delegate void GameMinute(TimeSpan time);
    //public static event GameMinute OnGameMinute;
    //public UnityEvent<TimeSpan> OnGameMinute;
    //public UnityEvent OnGameStart;
    [SerializeField] GameEvent OnGameUpdate;
    [SerializeField] GameEvent OnGameSecond;
    [SerializeField] GameEvent OnGameMinute;
    [SerializeField] GameEvent OnGameStart;

    public string TimeFormat = "mm\\:ss";
    public TimeSpan DisplayTime;
    private void Awake()
    {
        if (Instance != null) Debug.LogError("Only one Timer!");
        else Instance = this;
        StartCoroutine(nameof(GameUpdate));
    }

    IEnumerator GameUpdate()
    {
        Debug.Log(Time.deltaTime);
        yield return new WaitForSeconds(0.2f);
    }

    void Start()
    {
        Debug.Log("Game started!");
        OnGameStart.Raise(this, null);
        if (TimeObject != null) TimeObject.text = new DateTime().ToString(TimeFormat);
        InvokeRepeating(nameof(OnSecond), 0, 1);
    }

    private void OnSecond()
    {
        DisplayTime = DisplayTime.Add(new TimeSpan(0, 0, 1));
        if(TimeObject != null) TimeObject.text = DisplayTime.ToString(TimeFormat);
        OnGameSecond.Raise(this, DisplayTime);
        if (DisplayTime.Seconds % 60 == 0) OnGameMinute.Raise(this, DisplayTime);
    }
}

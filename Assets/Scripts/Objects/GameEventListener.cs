using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class CustomGameEvent : UnityEvent<Component, object> { }

public class GameEventListener : MonoBehaviour
{
    public GameEvent gameEvent;
    public CustomGameEvent response = new();
    public CustomGameEvent responseOnce = new();

    private void Awake()
    {
        gameEvent?.RegisterListener(this);
    }
    private void OnDisable()
    {
        gameEvent?.UnregisterListener(this);
    }

    public void OnEventRaised(Component sender, object data)
    {
        response?.Invoke(sender, data);
        responseOnce?.Invoke(sender, data);
        for (int i = 0; i < responseOnce.GetPersistentEventCount(); i++)
        {
            responseOnce.SetPersistentListenerState(i, UnityEventCallState.Off);
        }
    }
}

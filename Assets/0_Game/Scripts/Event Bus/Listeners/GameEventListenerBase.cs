using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class GameEventListenerBase<T> : MonoBehaviour
{
    public GameEventBaseSO<T> GameEvent;
    public UnityEvent<T> Response;

    private void OnEnable()
    {
        GameEvent.Event += GameEvent_Event;
    }

    private void OnDisable()
    {
        GameEvent.Event -= GameEvent_Event;
    }

    private void GameEvent_Event(T obj)
    {
        Response?.Invoke(obj);
    }
}

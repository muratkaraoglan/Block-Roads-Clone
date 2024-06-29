using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameEventBaseSO<T> : ScriptableObject
{
    public event System.Action<T> Event;

    public void RaiseEvent(T eventData)
    {
        Event?.Invoke(eventData);
    }

}

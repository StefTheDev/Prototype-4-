using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Event
{
    [SerializeField] private string name;
    private UnityEvent unityEvent;

    public Event()
    {
        unityEvent = new UnityEvent();
        unityEvent.AddListener(OnEvent);
        Debug.Log("Event created.");
    }

    public void Call()
    {
        if(unityEvent == null) { Debug.Log("Event null"); }
        unityEvent.Invoke();
    }

    public abstract void OnEvent();
}

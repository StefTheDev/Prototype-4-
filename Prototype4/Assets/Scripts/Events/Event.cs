using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Event : MonoBehaviour
{
    [SerializeField] private string name;
    private UnityEvent unityEvent;

    private void Start()
    {
        if (unityEvent == null) unityEvent = new UnityEvent();
        unityEvent.AddListener(OnEvent);
    }

    public void Call()
    {
        unityEvent.Invoke();
    }

    public abstract void OnEvent();
}

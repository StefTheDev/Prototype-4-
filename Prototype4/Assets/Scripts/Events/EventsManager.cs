using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventsManager : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private int delay = 10;
    [SerializeField] private List<Event> events;

    private Event lastEvent;
    private float time;
    private bool stop = true;
    private Queue<Event> eventQueue;

    private void Start()
    {
        eventQueue = new Queue<Event>();
        time = delay;

        foreach (Event @event in events)
        {
            eventQueue.Enqueue(@event);
        }
    }

    private void Update()
    {
        slider.value = time / delay;

        if (stop) return;
        time -= Time.deltaTime;

        if (time <= 0)
        {
            if (eventQueue.Count > 1) time = delay;
            if (eventQueue.Count > 0)
            {
                if(lastEvent != null) lastEvent.Call(EventState.END);
                lastEvent = eventQueue.Dequeue();
                lastEvent.Call(EventState.START);
            }
        }
    }

    public void Run(bool running)
    {
        stop = running;
        time = delay;
    }
}

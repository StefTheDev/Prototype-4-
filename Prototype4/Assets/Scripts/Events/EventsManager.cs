using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventsManager : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private int delay = 10;

    private float time;
    private bool stop = true;
    private Queue<Event> events;

    private void Start()
    {
        events = new Queue<Event>();
        time = delay;
    }

    private void Update()
    {
        slider.value = time / delay;

        if (stop) return;
        time -= Time.deltaTime;

        if (time <= 0)
        {
            if(events.Count <= 0)
            {
                //SUDDEN Death
                return;
            }
            events.Dequeue().Call();
            time = delay;
        }
    }

    public void Run(bool running)
    {
        stop = running;
        time = delay;
    }

    public Queue<Event> GetEvents()
    {
        return events;
    }
}

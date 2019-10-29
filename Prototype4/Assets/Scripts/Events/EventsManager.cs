using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventsManager : MonoBehaviour
{
    [SerializeField] private int delay = 10;
    [SerializeField] private bool stop = true;
    private Queue<Event> events;

    private void Start()
    {
        events = new Queue<Event>();

        for (int i = 0; i < 5; i++)
        {
            events.Enqueue(new LightningEvent());
        }
    }

    public void Run()
    {
        stop = false;
        StartCoroutine(ExecuteAfterTime(delay));
    }

    IEnumerator ExecuteAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        if (events.Count <= 0) stop = true;
        if (!stop) { 
            events.Dequeue().Call();
            StartCoroutine(ExecuteAfterTime(time));
        }
    }

    public Queue<Event> GetEvents()
    {
        return events;
    }
}

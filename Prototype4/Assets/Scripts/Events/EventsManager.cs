using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventsManager : MonoBehaviour
{
    [SerializeField] private int delay;
    private Queue<Event> events;

    private void Start()
    {
        events = new Queue<Event>();
    }

    public void Run()
    {
        StartCoroutine(ExecuteAfterTime(delay));
    }

    IEnumerator ExecuteAfterTime(float time)
    {
        yield return new WaitForSeconds(time);

        events.Dequeue().Call();
        StartCoroutine(ExecuteAfterTime(time));
    }

    public Queue<Event> GetEvents()
    {
        return events;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventsManager : MonoBehaviour
{
    [SerializeField] private int delay = 10;
    [SerializeField] private Slider slider;

    private bool stop = true;
    private Queue<Event> events;

    private void Start()
    {
        events = new Queue<Event>();
    }

    private void Update()
    {
        
    }

    public void Run()
    {
        stop = false;
        //Select random events and enqueue them. 

        //Temp Code
        for(int i = 0; i < 10; i++)
        {

        }

        StartCoroutine(ExecuteAfterTime(delay));
    }

    public void Stop()
    {
        Debug.Log("Events Stopped.");
        stop = true;
    }

    IEnumerator ExecuteAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        if (events.Count <= 0) Stop();
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

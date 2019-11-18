using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EventsManager : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private TMP_Text text;
    [SerializeField] private Animator animator;
    [SerializeField] private List<Event> events;

    private Event currentEvent;
    private float time;
    private Queue<Event> eventQueue;
    bool isActive = true;

    private static EventsManager instance;
    public static EventsManager Instance { get { return instance; } }

    private void Start()
    {
        if (instance != null && instance != this) { Destroy(this.gameObject); }
        else { instance = this; }

        eventQueue = new Queue<Event>();

        foreach (Event @event in events)
        {
            eventQueue.Enqueue(@event);
        }

        currentEvent = eventQueue.Dequeue();
        currentEvent.Call(EventState.START);

        time = currentEvent.GetDelay();
        text.text = currentEvent.GetDescription();

        slider.gameObject.SetActive(true);
        slider.image.sprite = eventQueue.Peek().GetSprite();
        Debug.Log(eventQueue.Peek().GetSprite().name);
    }

    private void Update()
    {
        if (!isActive) return;

        time -= Time.deltaTime;
        slider.value = time / currentEvent.GetDelay();

        if (currentEvent.name != "Sudden Death")
        {
            if (time <= currentEvent.GetDelay() / 2) animator.SetBool("Open", false);
        }
        

        if (Input.GetKey(KeyCode.T)) { time -= Time.deltaTime * 14; }

        if (time <= 0)
        {
            if (eventQueue.Count > 0)
            {
                if(currentEvent != null) currentEvent.Call(EventState.END);
                currentEvent = eventQueue.Dequeue();
                currentEvent.Call(EventState.START);

                time = currentEvent.GetDelay();
                text.text = currentEvent.GetDescription();

                if (eventQueue.Count > 0) slider.image.sprite = eventQueue.Peek().GetSprite();

                if (currentEvent.name != "Sudden Death")
                {
                    animator.SetBool("Open", true);
                }
            }
        }
    }

    public void SetActive(bool active)
    {
        this.isActive = active;
    }
}

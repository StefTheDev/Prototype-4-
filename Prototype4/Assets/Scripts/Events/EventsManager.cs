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

    private void Start()
    {
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
    }

    private void Update()
    {
        time -= Time.deltaTime;
        slider.value = time / currentEvent.GetDelay();

        if (time <= currentEvent.GetDelay() / 2) animator.SetBool("Open", false);

        if (time <= 0)
        {
            if (eventQueue.Count > 0)
            {
                if(currentEvent != null) currentEvent.Call(EventState.END);
                currentEvent = eventQueue.Dequeue();
                currentEvent.Call(EventState.START);

                time = currentEvent.GetDelay();
                text.text = currentEvent.GetDescription();
                animator.SetBool("Open", true);
            }
        }
    }
}

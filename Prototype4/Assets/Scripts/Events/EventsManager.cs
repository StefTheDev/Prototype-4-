﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EventsManager : MonoBehaviour
{
#pragma warning disable CS0649
    [SerializeField] private Slider slider;
    [SerializeField] private TMP_Text text;
    [SerializeField] private List<Event> events;
#pragma warning restore CS0649

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
        slider.value = time / currentEvent.GetDelay();
        time -= Time.deltaTime;

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
            }
        }
    }
}
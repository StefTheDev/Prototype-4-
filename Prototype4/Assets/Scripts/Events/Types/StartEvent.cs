using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartEvent : Event
{
    public override void OnEnd()
    {
        //Wait for the countdown to end.
    }

    public override void OnStart()
    {
        Debug.Log("Game has started");
        EventsManager.Instance.SetActive(true);
    }
}

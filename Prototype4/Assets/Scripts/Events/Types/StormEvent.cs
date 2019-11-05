using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StormEvent : Event
{
    [SerializeField] private GameObject lightning, rain;

    public bool includeChildren = true;

    public override void OnStart()
    {
        Debug.Log("Storm Event Started.");
        lightning.SetActive(true);
        rain.SetActive(true);
    }

    public override void OnEnd()
    {
        Debug.Log("Storm Event Ended.");
        lightning.SetActive(false);
        rain.SetActive(false);
    }
}

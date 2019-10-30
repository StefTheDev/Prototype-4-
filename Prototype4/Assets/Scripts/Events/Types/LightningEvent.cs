using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningEvent : Event
{
    public override void OnEvent()
    {
        Debug.Log("Boom!");
    }
}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartEvent : Event
{
    public override void OnEnd()
    {
        //Nothing...?
    }

    public override void OnStart()
    {
        Debug.Log("Game has started");
        //GameManager Starts game
    }
}
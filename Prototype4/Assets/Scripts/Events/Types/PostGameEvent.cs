﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PostGameEvent : Event
{
    public override void OnEnd()
    {
        //reloads the current scene. 
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public override void OnStart()
    {

    }
}

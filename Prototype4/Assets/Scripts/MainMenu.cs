﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string gameLevelName = "TimScene";

    public void OnPressPlay()
    {
        SceneManager.LoadSceneAsync(gameLevelName);
    }

    public void OnPressQuit()
    {
        Application.Quit();
    }
}

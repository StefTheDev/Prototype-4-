using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string gameLevelName = "TimScene";
    public GameObject options, main;

    public void OnPressPlay()
    {
        SceneManager.LoadScene(gameLevelName);
    }

    public void OnOptions()
    {
        main.SetActive(false);
        options.SetActive(true);
    }

    public void OnPressQuit()
    {
        Application.Quit();
    }
}

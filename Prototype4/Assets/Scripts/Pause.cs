using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    public Image[] images;
    public GameObject panel;
    public Image image;

    public string menuLevel = "MenuScene";


    private bool paused = false;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(paused)
            {
                Time.timeScale = 1;

                paused = false;
                panel.SetActive(false);

                image.color = images[0].color;
            }
            else
            {
                Time.timeScale = 0;

                paused = true;
                panel.SetActive(true);

                image.color = images[1].color;
            }
        }
    } 

    public void OnContinue()
    {
        Time.timeScale = 1;

        paused = false;
        panel.SetActive(false);

        image.color = images[0].color;
    }

    public void OnQuit()
    {
        SceneManager.LoadScene(menuLevel);
    }
}

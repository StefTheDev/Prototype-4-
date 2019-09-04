using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    public Image[] images;
    public GameObject panel, options;
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

                image.sprite = images[0].sprite;
            }
            else
            {
                Time.timeScale = 0;

                paused = true;
                panel.SetActive(true);

                image.sprite = images[1].sprite;
            }
        }
    } 

    public void OnContinue()
    {
        Time.timeScale = 1;

        paused = false;
        panel.SetActive(false);

        image.sprite = images[0].sprite;
    }

    public void OnOptions()
    {
        options.SetActive(true);
        gameObject.SetActive(false);
    }

    public void OnQuit()
    {
        SceneManager.LoadScene(menuLevel);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.SceneManagement;
using EventSystem = UnityEngine.EventSystems.EventSystem;

public class Pause : MonoBehaviour
{
    public Image[] images;
    public GameObject panel, options, joinPrompts, timer;
    public GameObject continueButton;
    public GameObject optionsBackButton;
    public Image image;

    public string menuLevel = "MenuScene";


    private bool paused = false;

    private EventSystem eventSystem;

    private void Awake()
    {
        eventSystem = FindObjectOfType<EventSystem>();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(paused)
            {
                Time.timeScale = 1;

                paused = false;
                panel.SetActive(false);

                joinPrompts.SetActive(true);
                timer.SetActive(true);

                image.sprite = images[0].sprite;
            }
            else
            {
                Time.timeScale = 0;

                paused = true;
                panel.SetActive(true);

                joinPrompts.SetActive(false);
                timer.SetActive(false);

                image.sprite = images[1].sprite;

                eventSystem.SetSelectedGameObject(continueButton);
            }
        }
    }

    public void OnContinue()
    {
        Time.timeScale = 1;

        paused = false;
        panel.SetActive(false);

        joinPrompts.SetActive(true);
        timer.SetActive(true);

        image.sprite = images[0].sprite;

        eventSystem.SetSelectedGameObject(null);
    }

    public void OnOptions()
    {
        options.SetActive(true);
        panel.SetActive(false);
        
        eventSystem.SetSelectedGameObject(optionsBackButton);
    }

    public void OnOptionsBack()
    {
        eventSystem.SetSelectedGameObject(continueButton);
    }

    public void OnQuit()
    {
        Time.timeScale = 1;

        SceneManager.LoadScene(menuLevel);
    }
}

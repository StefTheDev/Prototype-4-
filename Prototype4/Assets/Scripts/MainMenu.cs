using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string gameLevelName = "TimScene";
    public GameObject playButton;
    public GameObject options, main;
    public GameObject optionsBackButton;
    public GameObject instructionsPanel;
    public GameObject instructionsBackButton;

    public void OnPressPlay()
    {
        SceneManager.LoadScene(gameLevelName);
    }

    public void OnOptions()
    {
        main.SetActive(false);
        options.SetActive(true);

        FindObjectOfType<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(optionsBackButton);
    }

    public void OnInstructions()
    {
        main.SetActive(false);
        instructionsPanel.SetActive(true);

        FindObjectOfType<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(instructionsBackButton);
    }

    public void OnMain()
    {
        main.SetActive(true);
        options.SetActive(false);
        instructionsPanel.SetActive(false);

        FindObjectOfType<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(playButton);
    }

    public void OnPressQuit()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.PostProcessing;

public enum TimeState
{
    DAY,
    NIGHT
}

public class MainMenu : MonoBehaviour
{
    public string gameLevelName = "TimScene";
    public GameObject playButton;
    public GameObject main;
    // public GameObject optionsBackButton;
    public GameObject instructionsPanel;
    public GameObject instructionsBackButton;

    private float time = 0;
    private TimeState timeState = TimeState.NIGHT;

    public PostProcessVolume volume;
    private ColorGrading colorGrading;


    private void Start()
    {
        time = 8;
        colorGrading = volume.profile.GetSetting<ColorGrading>();
    }

    private void Update()
    {
        Debug.Log(timeState.ToString());
        time -= Time.deltaTime;
        if(time <= 0)
        {
            if(timeState == TimeState.DAY)
            {
                timeState = TimeState.NIGHT;
            } else
            {
                timeState = TimeState.DAY;
            }
            time = 10;
        }

        if (timeState == TimeState.DAY)
        {
            if (colorGrading.temperature.value < 16.0f) colorGrading.temperature.value += Time.deltaTime * 8;
            if (colorGrading.postExposure.value < 1.04f) colorGrading.postExposure.value += Time.deltaTime / 2;
        }
        else
        {
            if (colorGrading.temperature.value > -60.0f) colorGrading.temperature.value -= Time.deltaTime * 8;
            if (colorGrading.postExposure.value > 0.0f) colorGrading.postExposure.value -= Time.deltaTime / 2;
        }
    }

    public void OnPressPlay()
    {
        SceneManager.LoadScene(gameLevelName);
    }

    //public void OnOptions()
    //{
    //    main.SetActive(false);
    //    options.SetActive(true);

    //    FindObjectOfType<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(optionsBackButton);
    //}

    public void OnInstructions()
    {
        main.SetActive(false);
        instructionsPanel.SetActive(true);

        FindObjectOfType<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(instructionsBackButton);
    }

    public void OnMain()
    {
        main.SetActive(true);
        // options.SetActive(false);
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

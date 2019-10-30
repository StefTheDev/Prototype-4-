using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIOptions : MonoBehaviour
{
    public GameObject pausePanel;
    public GameObject playButton;

    public void Back()
    {
        gameObject.SetActive(false);
        pausePanel.SetActive(true);

        FindObjectOfType<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(playButton);
    }
}

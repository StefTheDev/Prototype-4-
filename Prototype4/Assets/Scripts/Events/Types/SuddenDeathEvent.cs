using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SuddenDeathEvent : Event
{
    public override void OnEnd()
    {
    
    }

    public override void OnStart()
    {
        GameManager gameManager = GameManager.Instance;
        gameManager.gameState = GameState.suddenDeath;
        AirBlast.SetSuddenDeath(true);

        gameManager.gameMusic.SetActive(false);
        gameManager.suddenDeathCanvas.SetActive(true);
        gameManager.suddenDeathMusic.SetActive(true);

        EventsManager.Instance.SetActive(false);
    }
}

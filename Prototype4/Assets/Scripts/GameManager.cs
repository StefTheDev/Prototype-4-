using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Cinemachine;

public enum GameState
{
    preGame,
    inGame,
    suddenDeath,
    postGame
}

public class GameManager : MonoBehaviour
{
    // Singleton
    private static GameManager instance;

    public static GameManager Instance { get { return instance; } }

    public TextMeshProUGUI timerText;

    public SpawnPoints spawnPoints;

    public float preGameLength = 5.0f;
    public float roundLength = 60.0f;
    public float postGameLength = 5.0f;

    private float preGameTimer = 0.0f;
    private float roundTimer = 0.0f;
    private float postGameTimer = 0.0f;

    public GameState gameState = GameState.preGame;

    public const int numPlayers = 4;
    public List<GameObject> playerManagers;
    public GameObject gameMusic;
    public GameObject timerObject;

    private void Awake()
    {
        if (instance != null && instance != this) { Destroy(this.gameObject); }
        else { instance = this; }
        Random.InitState((int)System.DateTime.Now.Ticks);
    }

    private void Start()
    {
        var playerManagerPrefab = ReferenceManager.Instance.playerManagerPrefab;

        // Create player managers
        for (int i = 0; i < numPlayers; i++)
        {
            var newManager = Instantiate(playerManagerPrefab, this.transform);
            var managerComp = newManager.GetComponent<PlayerManager>();

            managerComp.SetAI(true);

            playerManagers.Add(newManager);
            managerComp.SetPlayerID(i);
            managerComp.SpawnPlayer();
            
            managerComp.myPlayer.GetComponent<PlayerController>().Disable();
        }

        preGameTimer = preGameLength;
    }

    private void Update()
    {
        switch (gameState)
        {
            case GameState.preGame:
            {

                break;
            }

            case GameState.inGame:
            {
                roundTimer -= Time.deltaTime;
                timerText.text = roundTimer.ToString("#.##");

                if (roundTimer <= 0.0f)
                {
                    StartSuddenDeath();
                }

                break;
            }

            case GameState.suddenDeath:
            {
                timerText.text = "0.00";

                break;
            }

            case GameState.postGame:
            {
                postGameTimer -= Time.deltaTime;
                timerText.text = postGameTimer.ToString("#.##");
                
                if (postGameTimer <= 0.0f)
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                }

                break;
            }
        }
    }

    // Changes from the preGame state to the inGame state
    public void StartGame()
    {
        // Enable player controls
        foreach (GameObject manager in playerManagers)
        {
            manager.GetComponent<PlayerManager>().myPlayer.GetComponent<PlayerController>().Enable();
        }

        gameMusic.SetActive(true);
        timerObject.SetActive(true);

        roundTimer = roundLength;
        gameState = GameState.inGame;
    }

    // Changes from the inGame state to the suddenDeath state
    public void StartSuddenDeath()
    {
        gameState = GameState.suddenDeath;
        AirBlast.SetSuddenDeath(true);
    }

    // Changes to the postGame state
    public void EndGame()
    {
        // Disable player controls
        foreach (GameObject manager in playerManagers)
        {
            manager.GetComponent<PlayerManager>().myPlayer.GetComponent<PlayerController>().Disable();
        }

        gameMusic.SetActive(false);
        AudioManager.Instance.PlaySound("Victory");

        AirBlast.SetSuddenDeath(false);

        postGameTimer = postGameLength;
        gameState = GameState.postGame;
    }

    // Gets called when a player is knocked out
    public void OnKnockout(int _knockedOutID)
    {
        int playersInNormalRealm = 0;
        Player winner = null;

        // Check for victory
        foreach (GameObject manager in playerManagers)
        {
            var managerComp = manager.GetComponent<PlayerManager>();
            if (!managerComp.inShadowRealm)
            {
                playersInNormalRealm++;
                winner = managerComp.myPlayer.GetComponent<Player>();
            }
        }

        if (playersInNormalRealm == 1)
        {
            EndGame();

            winner.transform.rotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);
            winner.ActivateVictoryCamera();
        }
        else if (playersInNormalRealm == 0)
        {
            EndGame();
            winner = playerManagers[0].GetComponent<PlayerManager>().myPlayer.GetComponent<Player>();

            winner.transform.rotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);
            winner.ActivateVictoryCamera();
        }
    }
}

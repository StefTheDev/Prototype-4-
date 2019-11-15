using Action = System.Action;
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
    public List<GameObject> playerManagerObjects;
    public List<PlayerManager> playerManagers;
    public GameObject gameMusic;
    public GameObject gameEvents;
    public GameObject timerObject;
    public GameObject victoryCanvas;

    public GameObject suddenDeathMusic;
    public GameObject suddenDeathCanvas;

    public Action onGameStarted;
    public Action onGameEnded;

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

            // Make all players AI
            managerComp.SetAI(true);

            playerManagerObjects.Add(newManager);
            playerManagers.Add(managerComp);
            managerComp.SetPlayerID(i);
            managerComp.SpawnPlayer();
            
            managerComp.myPlayer.GetComponent<PlayerControllerRigidbody>().SetDisabled(true);
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
                break;
            }

            case GameState.postGame:
            {
                // Debug.Log("GAME TIMER ENDED");

                postGameTimer -= Time.deltaTime;
                timerText.text = postGameTimer.ToString("#.##");
                
                if (postGameTimer <= 0.0f)
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                }

                break;
            }

            case GameState.suddenDeath:
            {
                CheckWinner();
                break;
            }
        }
    }

    // Changes from the preGame state to the inGame state
    public void StartGame()
    {
        // Enable player controls
        foreach (GameObject manager in playerManagerObjects)
        {
            manager.GetComponent<PlayerManager>().myPlayer.GetComponent<PlayerControllerRigidbody>().SetDisabled(false);
        }

        gameMusic.SetActive(true);
        //timerObject.SetActive(true);
        gameEvents.SetActive(true);
            
        roundTimer = roundLength;
        gameState = GameState.inGame;

        onGameStarted?.Invoke();
    }

    // Changes from the inGame state to the suddenDeath state
    /*public void StartSuddenDeath()
    {
        gameState = GameState.suddenDeath;
        AirBlast.SetSuddenDeath(true);

        gameMusic.SetActive(false);
        suddenDeathCanvas.SetActive(true);
        suddenDeathMusic.SetActive(true);
    }
    */

    // Changes to the postGame state
    public void EndGame()
    {
        // Disable player controls
        foreach (GameObject manager in playerManagerObjects)
        {
            manager.GetComponent<PlayerManager>().myPlayer.GetComponent<PlayerControllerRigidbody>().SetDisabled(true);
        }

        gameMusic.SetActive(false);
        victoryCanvas.SetActive(true);
        AudioManager.Instance.PlaySound("Victory");
        suddenDeathCanvas.SetActive(false);
        suddenDeathMusic.SetActive(false);

        AirBlast.SetSuddenDeath(false);

        postGameTimer = postGameLength;
        gameState = GameState.postGame;

        onGameEnded?.Invoke();
    }

    // Gets called when a player is knocked out
    public void OnKnockout(int _knockedOutID)
    {
        if (gameState == GameState.postGame) { return; }

        //int playersInNormalRealm = 0;
        //Player winner = null;

        //// Check for victory
        //Debug.Log("NEW VICTORY CONDITION HASN'T BEEN ADDED YET");
        //foreach (GameObject manager in playerManagers)
        //{
            
        //}

        //if (playersInNormalRealm == 1)
        //{
        //    EndGame();

        //    winner.transform.rotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);
        //    winner.ActivateVictoryCamera();
        //}
        //else if (playersInNormalRealm == 0)
        //{
        //    EndGame();
        //    winner = playerManagers[0].GetComponent<PlayerManager>().myPlayer.GetComponent<Player>();

        //    winner.transform.rotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);
        //    winner.ActivateVictoryCamera();
        //}
    }

    private void CheckWinner()
    {
        PlayerManager winner = null;
        int highest = 0;

        bool draw = false;

        foreach (PlayerManager manager in playerManagers)
        {
            if (manager.normalKills > highest)
            {
                winner = manager;
                highest = manager.normalKills;
                draw = false;
            }
            else if (manager.normalKills == highest)
            {
                draw = true;
            }
        }

        if (winner != null && !draw)
        {
            EndGame();

            Debug.Log("THE WINNER IS PLAYER " + winner.playerID + "WITH " + winner.normalKills + " KILLS");

            Player playerComp = winner.myPlayer.GetComponent<Player>();
            playerComp.transform.rotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);
            playerComp.ActivateVictoryCamera();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Singleton
    private static GameManager instance;

    public static GameManager Instance { get { return instance; } }

    [SerializeField]
    private Timer timer;
    //private List<Player> players;

    public SpawnPoints spawnPoints;

    public const int numPlayers = 4;
    public List<GameObject> playerManagers;
   

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
            playerManagers.Add(newManager);
            managerComp.SetPlayerID(i);
            managerComp.SpawnPlayer();
        }
    }

    public bool IsGameOver()
    {
        // return (timer.GetTime() < 0.0f) && players.Count < 0;
        return false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public int playerID;
    public bool inShadowRealm = false;
    public bool isAI = false;

    public GameObject myPlayer;

    private const string shadowRealmLayer = "Shadow Realm";
    private const string normalRealmLayer = "Normal Realm";

    public void SetPlayerID(int _playerID)
    {
        playerID = _playerID;
    }

    public void SpawnPlayer()
    {
        // Instantiate player
        if (isAI)
        {
            myPlayer = Instantiate(GameManager.Instance.AIPrefab, Vector3.zero, Quaternion.identity, null);
        }
        else
        {
            myPlayer = Instantiate(GameManager.Instance.humanPrefab, Vector3.zero, Quaternion.identity, null);
        }

        myPlayer.GetComponent<Player>().SetPlayerID(playerID);

        RespawnPlayer();
    }

    public void RespawnPlayer()
    {
        bool foundFreeSpawn = false;
        SpawnPoint spawnPoint;

        do
        {
            spawnPoint = GameManager.Instance.spawnPoints.GetRandom();

            if (!spawnPoint.IsOccupied()) { foundFreeSpawn = true; }
        }
        while (!foundFreeSpawn);

        spawnPoint.Teleport(myPlayer.transform);
    }

    public void PlayerDeath()
    {
        SwitchRealm(true);

        RespawnPlayer();
    }

    public void SwitchRealm(bool _shadowRealm)
    {
        inShadowRealm = _shadowRealm;
        
        if (inShadowRealm)
        {
            myPlayer.layer = LayerMask.NameToLayer(shadowRealmLayer);
            myPlayer.GetComponent<Player>().ChangeRealm(true);
        }
        else
        {
            myPlayer.layer = LayerMask.NameToLayer(normalRealmLayer);
            myPlayer.GetComponent<Player>().ChangeRealm(false);
        }
    }
}

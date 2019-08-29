using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public int playerID;
    public bool inShadowRealm = false;
    public bool isAI = false;

    public int normalKills = 0;
    public int shadowKills = 0;

    public GameObject myPlayer;
    private Player myPlayerComp;

    private const string shadowRealmLayer = "Shadow Realm";
    private const string normalRealmLayer = "Normal Realm";

    private const int spawnTries = 100;

    // ID of the last player that hit us
    private int lastHitBy = -1;
    

    public void SetPlayerID(int _playerID)
    {
        playerID = _playerID;
    }

    public void SpawnPlayer()
    {
        // Instantiate player
        if (isAI)
        {
            myPlayer = Instantiate(ReferenceManager.Instance.AIPrefab, Vector3.zero, Quaternion.identity, null);
        }
        else
        {
            myPlayer = Instantiate(ReferenceManager.Instance.humanPrefab, Vector3.zero, Quaternion.identity, null);
        }

        myPlayerComp = myPlayer.GetComponent<Player>();

        myPlayerComp.SetPlayerID(playerID);

        RespawnPlayer();
    }

    public void AwardKill(bool _isShadowKill)
    {
        if (_isShadowKill)
        {
            shadowKills++;
            SwitchRealm(false);
            // RespawnPlayer();
        }
        else
        {
            normalKills++;
        }
    }

    public void RespawnPlayer()
    {
        bool foundFreeSpawn = false;
        SpawnPoint spawnPoint;

        var attempts = 0;

        do
        {
            // If failed 100 times, just spawn at the first spawn point, to prevent infinite loop
            if (attempts > spawnTries)
            {
                spawnPoint = GameManager.Instance.spawnPoints.spawnPoints[0];
                break;
            }

            spawnPoint = GameManager.Instance.spawnPoints.GetRandom();

            if (!spawnPoint.IsOccupied()) { foundFreeSpawn = true; }

            attempts++;
        }
        while (!foundFreeSpawn);

        spawnPoint.Teleport(myPlayer.transform);
        myPlayer.GetComponent<Rigidbody>().velocity = Vector3.zero;
    }

    public void PlayerDeath()
    {
        if (lastHitBy != -1)
        {
            GameManager.Instance.playerManagers[lastHitBy].GetComponent<PlayerManager>().AwardKill(inShadowRealm);
        }

        if (!inShadowRealm) { SwitchRealm(true); }

        RespawnPlayer();
    }

    public void SwitchRealm(bool _shadowRealm)
    {
        inShadowRealm = _shadowRealm;
        
        if (inShadowRealm)
        {
            myPlayer.layer = LayerMask.NameToLayer(shadowRealmLayer);
            myPlayerComp.ChangeRealm(true);
        }
        else
        {
            myPlayer.layer = LayerMask.NameToLayer(normalRealmLayer);
            myPlayerComp.ChangeRealm(false);
        }
    }

    public void SetLastHitBy(int _playerID)
    {
        lastHitBy = _playerID;
    }
}

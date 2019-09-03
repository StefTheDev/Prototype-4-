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

    private string[] actionButtons = { "P1_Charge", "P2_Charge", "P3_Charge", "P4_Charge" };
    private string[] backButtons = { "P1_Back", "P2_Back", "P3_Back", "P4_Back" };

    private const int spawnTries = 100;

    // ID of the last player that hit us
    private int lastHitBy = -1;

    public void SetPlayerID(int _playerID)
    {
        playerID = _playerID;
    }

    private void Update()
    {
        if (Input.GetButtonDown(actionButtons[playerID]) && isAI)
        {
            HumanJoin();
        }

        if (Input.GetButtonDown(backButtons[playerID]) && !isAI)
        {
            HumanLeave();
        }
    }

    public void SpawnPlayer()
    {
        // Instantiate player
        if (isAI)
        {
            myPlayer = Instantiate(ReferenceManager.Instance.AIPlayerPrefabs[playerID], Vector3.zero, Quaternion.identity, null);
        }
        else
        {
            myPlayer = Instantiate(ReferenceManager.Instance.humanPlayerPrefabs[playerID], Vector3.zero, Quaternion.identity, null);
        }

        myPlayerComp = myPlayer.GetComponent<Player>();

        myPlayerComp.SetPlayerID(playerID);
        myPlayerComp.inShadowRealm = this.inShadowRealm;
        myPlayerComp.UpdateMaterial();

        RespawnPlayer();
    }

    public void AwardKill(bool _isShadowKill)
    {
        if (_isShadowKill)
        {
            shadowKills++;
            SwitchRealm(false);
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
        lastHitBy = -1;

        var attempts = 0;

        do
        {
            // If failed 100 times, just spawn at the first spawn point, to prevent infinite loop
            if (attempts > spawnTries)
            {
                spawnPoint = GameManager.Instance.spawnPoints.GetRandom();
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

        GameManager.Instance.OnKnockout(playerID);

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

    public void SetAI(bool _isAI)
    {
        isAI = _isAI;
    }

    private void HumanJoin()
    {
        isAI = false;
        bool inputsDisabled = myPlayer.GetComponent<PlayerController>().disabled;
        Destroy(myPlayer);
        SpawnPlayer();
        if (inputsDisabled)
        {
            myPlayer.GetComponent<PlayerController>().Disable();
        }

        ReferenceManager.Instance.joinPrompts[playerID].SetActive(false);
    }

    private void HumanLeave()
    {
        isAI = true;
        bool inputsDisabled = myPlayer.GetComponent<PlayerController>().disabled;
        Destroy(myPlayer);
        SpawnPlayer();
        if (inputsDisabled)
        {
            myPlayer.GetComponent<PlayerController>().Disable();
        }

        ReferenceManager.Instance.joinPrompts[playerID].SetActive(true);
    }
}

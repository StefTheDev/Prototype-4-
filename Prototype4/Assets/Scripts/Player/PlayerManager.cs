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
    private PlayerControllerRigidbody controller;

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
        controller = myPlayer.GetComponent<PlayerControllerRigidbody>();

        RespawnPlayer();
    }

    public void AwardKill(bool _isShadowKill)
    {
        if (_isShadowKill)
        {
            shadowKills++;
            SwitchRealm(false);
            controller.PlayReturnOnKillSound();
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

        myPlayerComp.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        spawnPoint.Teleport(myPlayer.transform);
        myPlayer.GetComponent<Rigidbody>().velocity = Vector3.zero;

        myPlayer.GetComponent<ShieldPowerup>().EndEffects();
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
            int newLayer = LayerMask.NameToLayer(shadowRealmLayer);
            myPlayer.layer = newLayer;
            foreach (Transform trans in gameObject.GetComponentsInChildren<Transform>(true))
            {
                trans.gameObject.layer = newLayer;
            }
            myPlayerComp.ChangeRealm(true);
        }
        else
        {
            int newLayer = LayerMask.NameToLayer(normalRealmLayer);
            myPlayer.layer = newLayer;
            foreach (Transform trans in gameObject.GetComponentsInChildren<Transform>(true))
            {
                trans.gameObject.layer = newLayer;
            }
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

        bool inputsDisabled = myPlayer.GetComponent<PlayerControllerRigidbody>().IsDisabled();

        Destroy(myPlayer);
        SpawnPlayer();

        if (inputsDisabled)
        {
            myPlayer.GetComponent<PlayerControllerRigidbody>().SetDisabled(true);
        }

        ReferenceManager.Instance.joinPrompts[playerID].SetActive(false);
    }

    private void HumanLeave()
    {
        isAI = true;
        bool inputsDisabled = myPlayer.GetComponent<PlayerControllerRigidbody>().IsDisabled();
        Destroy(myPlayer);
        SpawnPlayer();

        if (inputsDisabled)
        {
            myPlayer.GetComponent<PlayerControllerRigidbody>().SetDisabled(true);
        }

        ReferenceManager.Instance.joinPrompts[playerID].SetActive(true);
    }
}

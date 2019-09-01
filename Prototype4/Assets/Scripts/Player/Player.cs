using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int playerID;
    public bool inShadowRealm = false;
    public GameObject victoryCamera;

    public AudioClip deathSound;

    public AudioSource audioSource;

    public int GetPlayerID()
    {
        return playerID;
    }

    public void SetPlayerID(int _newID)
    {
        playerID = _newID;
    }

    public void ChangeRealm(bool _shadowRealm)
    {
        if (_shadowRealm && !inShadowRealm)
        {
            audioSource.PlayOneShot(deathSound);
        }
        else if (!_shadowRealm && inShadowRealm)
        {
            
        }

        inShadowRealm = _shadowRealm;
        UpdateMaterial();
    }

    public void UpdateMaterial()
    {
        if (inShadowRealm)
        {
            GetComponentInChildren<MeshRenderer>().material = ReferenceManager.Instance.playerShadowRealmMaterials[playerID];
        }
        else
        {
            GetComponentInChildren<MeshRenderer>().material = ReferenceManager.Instance.playerMaterials[playerID];
        }
    }

    public void ActivateVictoryCamera()
    {
        victoryCamera.SetActive(true);
    }
}

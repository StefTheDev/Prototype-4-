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
            GetComponentInChildren<MeshRenderer>().material = ReferenceManager.Instance.shadowRealmMaterial;
            audioSource.PlayOneShot(deathSound);
        }
        else if (!_shadowRealm && inShadowRealm)
        {
            GetComponentInChildren<MeshRenderer>().material = ReferenceManager.Instance.normalMaterial;
        }

        inShadowRealm = _shadowRealm;
    }

    public void ActivateVictoryCamera()
    {
        victoryCamera.SetActive(true);
    }
}

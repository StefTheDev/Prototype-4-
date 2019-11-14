using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int playerID;
    public GameObject victoryCamera;
    private GameObject fireParticles;

    public AudioClip deathSound;

    public AudioSource audioSource;

    public bool isInvulnerable;

    private PlayerControllerRigidbody controller;

    private void Start()
    {
        controller = GetComponent<PlayerControllerRigidbody>();
    }

    public int GetPlayerID()
    {
        return playerID;
    }

    public void SetPlayerID(int _newID)
    {
        playerID = _newID;
    }

    //public void ChangeRealm(bool _shadowRealm)
    //{
    //    if (_shadowRealm && !inShadowRealm)
    //    {
    //        audioSource.PlayOneShot(deathSound);
    //        controller.ghostParticles = Instantiate(ReferenceManager.Instance.fireParticlePrefabs[playerID], null);
    //    }
    //    else if (!_shadowRealm && inShadowRealm)
    //    {
    //        if (controller.ghostParticles) { Destroy(controller.ghostParticles); }
    //    }

    //    inShadowRealm = _shadowRealm;
    //    UpdateMaterial();
    //}

    //public void UpdateMaterial()
    //{
    //    var renderer = GetComponentInChildren<SkinnedMeshRenderer>();
    //    Material newMat = null;
    //    var rendererMaterials = renderer.materials;

    //    if (inShadowRealm)
    //    {
    //        newMat = new Material(ReferenceManager.Instance.playerShadowRealmMaterials[playerID]);
    //    }
    //    else
    //    {
    //        newMat = new Material(ReferenceManager.Instance.playerMaterials[playerID]);
    //    }

    //    rendererMaterials[0] = newMat;
    //    renderer.materials = rendererMaterials;
    //}

    public void ActivateVictoryCamera()
    {
        victoryCamera.SetActive(true);
    }
}

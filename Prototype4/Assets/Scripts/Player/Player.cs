using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int playerID;
    public bool inShadowRealm = false;

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
        inShadowRealm = _shadowRealm;

        var meshRenderer = GetComponentInChildren<MeshRenderer>();

        if (_shadowRealm)
        {
            GetComponentInChildren<MeshRenderer>().material = ReferenceManager.Instance.shadowRealmMaterial;
        }
        else
        {
            GetComponentInChildren<MeshRenderer>().material = ReferenceManager.Instance.normalMaterial;
        }
    }
}

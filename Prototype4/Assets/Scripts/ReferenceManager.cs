using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReferenceManager : MonoBehaviour
{
    // Singleton
    private static ReferenceManager instance;

    public static ReferenceManager Instance { get { return instance; } }

    public GameObject[] humanPlayerPrefabs;
    public GameObject[] AIPlayerPrefabs;
    public GameObject playerManagerPrefab;

    public Material[] playerMaterials;
    public Material[] playerShadowRealmMaterials;
    public GameObject[] joinPrompts;

    private void Awake()
    {
        if (instance != null && instance != this) { Destroy(this.gameObject); }
        else { instance = this; }
    }
}

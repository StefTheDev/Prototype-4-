using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReferenceManager : MonoBehaviour
{
    // Singleton
    private static ReferenceManager instance;

    public static ReferenceManager Instance { get { return instance; } }

    public GameObject humanPrefab;
    public GameObject AIPrefab;
    public GameObject playerManagerPrefab;

    public Material normalMaterial;
    public Material shadowRealmMaterial;

    private void Awake()
    {
        if (instance != null && instance != this) { Destroy(this.gameObject); }
        else { instance = this; }
    }
}

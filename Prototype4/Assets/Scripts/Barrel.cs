using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrel : MonoBehaviour
{
    public GameObject powerupPrefab;
    public ParticleSystem emitter;

    public void Break()
    {
        if (!this) { return; }

        var go = Instantiate(powerupPrefab, transform.position, Quaternion.identity, transform.parent);

        emitter.transform.SetParent(transform.parent);
        emitter.transform.localRotation = Quaternion.Euler(-90, 0, 0);
        emitter.gameObject.SetActive(true);

        Destroy(gameObject);
    }
}

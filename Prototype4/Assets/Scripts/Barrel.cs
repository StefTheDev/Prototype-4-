using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrel : MonoBehaviour
{
    public GameObject powerupPrefab;
    public ParticleSystem emitter;

    public GameObject unbrokenBarrel;
    public GameObject brokenBarrel;

    public void Break()
    {
        if (!this) { return; }

        var go = Instantiate(powerupPrefab, transform.position, Quaternion.identity, transform.parent);

        //emitter.transform.SetParent(transform.parent);
        //emitter.transform.localRotation = Quaternion.Euler(-90, 0, 0);
        //emitter.gameObject.SetActive(true);

        // Replace full mesh with broken mesh
        unbrokenBarrel.SetActive(false);
        brokenBarrel.SetActive(true);

        // Destroy fragments after 5 seconds
        Destroy(gameObject, 5.0f);
    }
}

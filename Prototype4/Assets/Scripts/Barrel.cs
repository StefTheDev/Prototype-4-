using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrel : MonoBehaviour
{
    public GameObject powerupPrefab;

    public GameObject unbrokenBarrel;
    public GameObject brokenBarrel;

    public void Break()
    {
        if (!this.isActiveAndEnabled) { return; }

        var go = Instantiate(powerupPrefab, transform.position, Quaternion.identity, transform.parent);

        AudioManager.Instance.PlaySound("BarrelBreak", 1.5f);

        // Replace full mesh with broken mesh
        brokenBarrel.transform.SetParent(transform.parent);
        brokenBarrel.SetActive(true);
        gameObject.SetActive(false);

        // Destroy fragments after 5 seconds
        Destroy(brokenBarrel, 5.0f);
        Destroy(gameObject, 5.0f);
    }
}

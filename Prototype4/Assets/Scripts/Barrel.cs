using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrel : MonoBehaviour
{
    public GameObject powerupPrefab;

    public GameObject unbrokenBarrel;
    public GameObject brokenBarrel;

    public void Break(Vector3 launchForce)
    {
        if (!this.isActiveAndEnabled) { return; }

        var go = Instantiate(powerupPrefab, transform.position, Quaternion.identity, transform.parent);

        AudioManager.Instance.PlaySound("BarrelBreak", 1.5f);

        // Replace full mesh with broken mesh
        brokenBarrel.transform.SetParent(transform.parent);
        brokenBarrel.SetActive(true);
        foreach (var rb in brokenBarrel.GetComponentsInChildren<Rigidbody>())
        {
            rb.AddForce(Random.Range(0.5f, 1.0f) * launchForce, ForceMode.Impulse);
        }
        gameObject.SetActive(false);

        // Destroy fragments after 5 seconds
        Destroy(brokenBarrel, 5.0f);
        Destroy(gameObject, 5.0f);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrel : MonoBehaviour
{
    public GameObject powerupPrefab;

    public GameObject unbrokenBarrel;
    public GameObject brokenBarrel;

    private Rigidbody rigidBody;

    private void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

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

    private void FixedUpdate()
    {
        const float waterY = -12.0f;
        const float arenaRadius = 10.0f;

        Vector3 pos = this.transform.position;
        float posMag = pos.magnitude;

        // If off arena edge
        if (posMag > arenaRadius + 0.2f)
        {
            if (pos.y < 0.0f)
            {
                float newScale = 1.0f - pos.y / waterY;
                this.transform.localScale = new Vector3(newScale, newScale, newScale);

                var currentVelocity = rigidBody.velocity;
                currentVelocity.y = 0.0f;
                currentVelocity *= -0.05f;
                rigidBody.velocity += currentVelocity;
            }
        }

        if (transform.position.y < -11.0f) { GameObject.Destroy(this.gameObject); }

    }
    
}

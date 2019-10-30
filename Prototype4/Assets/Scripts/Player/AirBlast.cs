using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class AirBlast : MonoBehaviour
{

    public static float blastSpeed = 1000.0f;
    public static float maxLifetime = 1.0f;
    public static float normalBlastForce = 750.0f;
    public static float suddenDeathBlastForce = 3000.0f;
    public static float verticalBlastForce = 100.0f;

    private Rigidbody rigidBody;
    private Vector3 direction;
    private int playerIndex;
    private float chargeAmount = 0.0f;
    private static float currentBlastForce = normalBlastForce;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
    }
    public void Launch(Vector3 _launchDirection, float _chargeAmount, int _playerIndex)
    {
        rigidBody.AddForce(_launchDirection * _chargeAmount * blastSpeed);
        this.transform.forward = _launchDirection;
        direction = _launchDirection;
        playerIndex = _playerIndex;
        chargeAmount = _chargeAmount;
        GameObject.Destroy(this.gameObject, maxLifetime * _chargeAmount);
    }

    private void OnTriggerEnter(Collider other)
    {
        var otherPlayer = other.GetComponent<Player>();

        var otherAirBlast = other.GetComponent<AirBlast>();

        if (otherAirBlast)
        {
            Destroy(otherAirBlast);
            Destroy(this.gameObject);
        }

        if (otherPlayer && (otherPlayer.GetPlayerID() != playerIndex) && !otherPlayer.isInvulnerable)
        {
            AudioManager.Instance.PlaySound("ShoutHit", 0.5f);

            // Always apply normal blast force to shadow realm players
            float blastForce = currentBlastForce;
            if (otherPlayer.inShadowRealm) { blastForce = normalBlastForce; }

            GameManager.Instance.playerManagers[otherPlayer.GetPlayerID()].GetComponent<PlayerManager>().SetLastHitBy(playerIndex);

            Vector3 launchForce = direction * blastForce * chargeAmount;

			// Edit by Elijah
			otherPlayer.GetComponent<ShieldPowerup>().ApplyAirBlast(new ShotHitInfo(transform.position, transform.forward), launchForce);

            // Launch player vertically
            otherPlayer.GetComponent<Rigidbody>().AddForce(Vector3.up * verticalBlastForce * chargeAmount);

            Destroy(this.gameObject);
        }
    }

    public static void SetSuddenDeath(bool _isSuddenDeath)
    {
        if (_isSuddenDeath)
        {
            currentBlastForce = suddenDeathBlastForce;
        }
        else
        {
            currentBlastForce = normalBlastForce;
        }
    }
}

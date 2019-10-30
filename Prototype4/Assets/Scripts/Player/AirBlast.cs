using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class AirBlast : MonoBehaviour
{

    public static float blastSpeed = 10.0f;
    public static float maxLifetime = 1.0f;
    public static float normalBlastForce = 7.5f;//750.0f;
    public static float suddenDeathBlastForce = 30.0f;//3000.0f;
    public static float verticalBlastForce = 3.0f;

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
        rigidBody.AddForce(_launchDirection * _chargeAmount * blastSpeed, ForceMode.VelocityChange);
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

        float blastForce = currentBlastForce;
        // Always apply normal blast force to shadow realm players
        if (otherPlayer && otherPlayer.inShadowRealm) { blastForce = normalBlastForce; }

        Vector3 launchForce = direction * blastForce * chargeAmount;

        if (otherPlayer && otherPlayer.GetPlayerID() != playerIndex && !otherPlayer.isInvulnerable)
        {
            GameManager.Instance.playerManagers[otherPlayer.GetPlayerID()].GetComponent<PlayerManager>().SetLastHitBy(playerIndex);
            
			// Edit by Elijah
			otherPlayer.GetComponent<ShieldPowerup>().ApplyAirBlast(new ShotHitInfo(transform.position, transform.forward), launchForce, chargeAmount);

            // Launch player vertically
            otherPlayer.GetComponent<Rigidbody>().AddForce(Vector3.up * verticalBlastForce * chargeAmount, ForceMode.Impulse);

            Destroy(this.gameObject);
        }

        if (!otherPlayer)
        {
            var otherRigidbody = other.GetComponent<Rigidbody>();
            if (otherRigidbody)
            {
                otherRigidbody.AddForce(launchForce, ForceMode.Impulse);
            }
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

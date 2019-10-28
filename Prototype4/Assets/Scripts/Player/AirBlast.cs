using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class AirBlast : MonoBehaviour
{
    public static float blastSpeed = 1000.0f;
    public static float maxLifetime = 1.0f;
    public static float blastForce = 750.0f;
    public static float verticalBlastForce = 100.0f;

    private Rigidbody rigidBody;
    private Vector3 direction;
    private int playerIndex;
    private float chargeAmount = 0.0f;

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

        if (otherPlayer && otherPlayer.GetPlayerID() != playerIndex && !otherPlayer.isInvulnerable)
        {
            GameManager.Instance.playerManagers[otherPlayer.GetPlayerID()].GetComponent<PlayerManager>().SetLastHitBy(playerIndex);
            Vector3 launchForce = direction * blastForce * chargeAmount;
            launchForce.y = verticalBlastForce * chargeAmount;
            otherPlayer.GetComponent<Rigidbody>().AddForce(launchForce);
			// Edit by Elijah
			otherPlayer.GetComponent<ShieldPowerup>().ApplyAirBlast(direction * blastForce * chargeAmount);

            Destroy(this.gameObject);
        }
    }

    public static void SetSuddenDeath(bool _isSuddenDeath)
    {
        if (_isSuddenDeath)
        {
            blastForce = 3000.0f;
        }
        else
        {
            blastForce = 500.0f;
        }
    }
}

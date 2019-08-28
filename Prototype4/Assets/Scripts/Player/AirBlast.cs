using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class AirBlast : MonoBehaviour
{
    public static float blastSpeed = 1000.0f;
    public static float maxLifetime = 1.0f;
    public static float blastForce = 300.0f;

    private Rigidbody rigidBody;
    private Vector3 direction;
    private int playerIndex;
    private float chargeAmount;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    public void Launch(Vector3 _launchDirection, float _chargeAmount, int _playerIndex)
    {
        rigidBody.AddForce(_launchDirection * _chargeAmount * blastSpeed);
        direction = _launchDirection;
        playerIndex = _playerIndex;
        chargeAmount = _chargeAmount;
        GameObject.Destroy(this.gameObject, maxLifetime * _chargeAmount);
    }

    private void OnTriggerEnter(Collider other)
    {
        var otherPlayer = other.GetComponent<Player>();

        if (otherPlayer && otherPlayer.GetPlayerID() != playerIndex)
        {
            otherPlayer.GetComponent<Rigidbody>().AddForce(direction * blastForce * chargeAmount);
            Destroy(this.gameObject);
        }
    }

    public static void SetSuddenDeath(bool _isSuddenDeath)
    {
        if (_isSuddenDeath)
        {
            blastForce = 100.0f;
        }
        else
        {
            blastForce = 10.0f;
        }
    }
}

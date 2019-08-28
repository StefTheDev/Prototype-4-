using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class AirBlast : MonoBehaviour
{
    public float blastSpeed = 10.0f;
    public float maxLifetime = 3.0f;
    public float blastForce = 10.0f;

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
}

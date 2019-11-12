﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class AirBlast : MonoBehaviour
{

    public static float blastSpeed = 10.0f;
    public static float maxLifetime = 1.0f;
    public static float normalBlastForce = 15.5f;//750.0f;
    public static float suddenDeathBlastForce = 40.0f;//3000.0f;
    public static float verticalBlastForce = 3.0f;

    private Rigidbody rigidBody;
    private Vector3 direction;
    private int playerIndex;
    private bool inShadowRealm;
    private float chargeAmount = 0.0f;
    private static float currentBlastForce = normalBlastForce;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
    }
    public void Launch(Vector3 _launchDirection, float _chargeAmount, int _playerIndex, bool _inShadowRealm)
    {
        rigidBody.AddForce(_launchDirection * _chargeAmount * blastSpeed, ForceMode.VelocityChange);
        this.transform.forward = _launchDirection;
        direction = _launchDirection;
        playerIndex = _playerIndex;
        inShadowRealm = _inShadowRealm;
        chargeAmount = _chargeAmount;
        GameObject.Destroy(this.gameObject, maxLifetime * _chargeAmount);
    }

    private void OnTriggerEnter(Collider other)
    {
        var otherPlayer = other.GetComponent<Player>();

        var otherAirBlast = other.GetComponent<AirBlast>();

        if (otherAirBlast)
        {
            if (otherAirBlast.playerIndex == this.playerIndex) { return; }

            AudioManager.Instance.PlaySound("AirBlastCollision", 2.0f);
            var particles = Instantiate(ReferenceManager.Instance.airBlastCollisionParticle, this.transform.position, this.transform.rotation);
            GameObject.Destroy(particles, 1.0f);
            Destroy(otherAirBlast);
            Destroy(this.gameObject);
            return;
        }

        float blastForce = currentBlastForce;
        // Always apply normal blast force to shadow realm players
        if (otherPlayer && otherPlayer.inShadowRealm) { blastForce = normalBlastForce; }

        Vector3 launchForce = direction * blastForce * chargeAmount;

        if (otherPlayer && otherPlayer.GetPlayerID() != playerIndex && !otherPlayer.isInvulnerable)
        {
            //Debug.Log("AirBlast Hit Player!" + System.DateTime.Now.Ticks);

            AudioManager.Instance.PlaySound("ShoutHit", 0.5f);
            GameManager.Instance.playerManagers[otherPlayer.GetPlayerID()].GetComponent<PlayerManager>().SetLastHitBy(playerIndex);
            
			// Edit by Elijah
			otherPlayer.GetComponent<ShieldPowerup>().ApplyAirBlast(new ShotHitInfo(transform.position, transform.forward), launchForce, chargeAmount);

            // Launch player vertically
            otherPlayer.GetComponent<Rigidbody>().AddForce(Vector3.up * verticalBlastForce * chargeAmount, ForceMode.Impulse);

            Destroy(this.gameObject);
        }

        if (!otherPlayer)
        {
            var otherRigidbody = other.GetComponentInParent<Rigidbody>();
            if (otherRigidbody)
            {
                if (otherRigidbody.GetComponent<Log>())
                {
                    launchForce *= 10.0f;
                    GameObject.Destroy(this.gameObject, 0.01f);
                }

                AudioManager.Instance.PlaySound("ShoutHit", 0.3f);
                otherRigidbody.AddForce(launchForce, ForceMode.Impulse);
            }

            var barrel = other.GetComponentInParent<Barrel>();
            if (barrel)
            {
                barrel.Break();
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

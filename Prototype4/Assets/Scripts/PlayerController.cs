using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    public float moveForce = 10.0f;
    public GameObject airBlastPrefab;

    // public float movingDrag = 10.0f;
    // public float normalDrag = 1.0f;

    public float chargeTime = 1.0f;
    
    public float currentCharge = 0.0f;
    private Rigidbody rigidBody;
    private Vector3 moveVector = Vector3.zero;
    private bool isCharging = false;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    public void Move(Vector3 _moveDirection)
    {
        moveVector = _moveDirection;
    }

    private void Update()
    {
        if (isCharging)
        {
            currentCharge = Mathf.Clamp(currentCharge + (Time.deltaTime / chargeTime), 0.0f, 1.0f);
        }
    }

    public void StartCharging()
    {
        isCharging = true;
        currentCharge = 0.0f;
    }

    public void FireProjectile()
    {
        isCharging = false;

        // Fire projectile
        var airBlast = Instantiate(airBlastPrefab, this.transform.position, Quaternion.identity, null);
        airBlast.GetComponent<AirBlast>().Launch(this.transform.forward, currentCharge, this.GetComponent<Player>().GetPlayerID());

        currentCharge = 0.0f;

    }

    private void FixedUpdate()
    {
        rigidBody.AddForce(moveVector.normalized * moveForce);
        
        if (moveVector != Vector3.zero)
        {
            transform.forward = moveVector;
        }

    }
}

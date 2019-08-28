using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    public float moveForce = 10.0f;

    private Rigidbody rigidBody;
    private Vector3 moveVector = Vector3.zero;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    public void Move(Vector3 _moveDirection)
    {
        moveVector = _moveDirection;
    }

    public void Push()
    {
        // If push pressed down, start charging

        // If push released, fire
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

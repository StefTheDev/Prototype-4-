using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPlayer : Player
{
    // Arena radius size
    public float arenaRadius = 10.0f;

    // How close I have to be to the edge to start turning away
    public float avoidDistance = 1.0f;

    public GameObject targetPlayer;

    private PlayerControllerRigidbody controller;
    private Rigidbody rigidBody;

    private Vector3 currentDirection = Vector3.forward;

    public float maxVelocity = 12.0f;

    public float attackDistance = 3.0f;

    public float maxSteeringForce = 0.1f;
    private Vector3 steeringForce;
    private Vector3 currentForce;

    private float attackTimer = 0.0f;

    public float changeTargetCheck = 0.1f;
    private float changeTargetTimer = -0.1f;

    private void Awake()
    {
        controller = GetComponent<PlayerControllerRigidbody>();
        rigidBody = GetComponent<Rigidbody>();
        changeTargetTimer = changeTargetCheck;
    }

    private void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (controller.IsDisabled()) { return; }

        steeringForce = Vector3.zero;

        changeTargetTimer -= Time.deltaTime;
        attackTimer -= Time.deltaTime;

        // Check for a new target
        if (changeTargetTimer <= 0.0f)
        {
            targetPlayer = FindClosestPlayer();
            changeTargetTimer = changeTargetCheck;
        }

        if (attackTimer < 0.0f && targetPlayer)
        {
            controller.SetLook(targetPlayer.transform.position - this.transform.position);
            controller.FireProjectile();
        }
        else if (attackTimer < 0.0f && !targetPlayer)
        {
            attackTimer = 0.1f;
        }

        // If there is another player currently in the same realm
        if (targetPlayer)
        {
            // If target is out of range, seek target
            if ((this.transform.position - targetPlayer.transform.position).magnitude > attackDistance)
            {
                steeringForce += Seek(targetPlayer.transform.position);
            }
            // If target is in range and we are not charging an attack, start charging an attack
            else if (attackTimer < 0.0f)
            {
                // controller.ApplyBrakingForce();
                controller.StartCharging();
                attackTimer = Random.Range(0.5f, 1.5f);
            }
            // If target is in range and we are charging an attack, keep looking at them
            else
            {
                // controller.ApplyBrakingForce();
                controller.SetLook(targetPlayer.transform.position - this.transform.position);
            }
        }

        steeringForce += Containment();

        steeringForce = Vector3.ClampMagnitude(steeringForce, maxSteeringForce);

        currentForce = Vector3.ClampMagnitude(currentForce + steeringForce, maxVelocity);
        controller.Move(currentForce);
    }

    private Vector3 Seek(Vector3 _seekPos)
    {
        Vector3 desiredVelocity = (_seekPos - this.transform.position);
        desiredVelocity.y = 0.0f;
        desiredVelocity = desiredVelocity.normalized * maxVelocity;

        var seekForce = (desiredVelocity - rigidBody.velocity);

        seekForce.y = 0.0f;

        return seekForce;
    }

    private Vector3 Containment()
    {
        var pos = this.transform.position;
        pos.y = 0.0f;

        if (pos.magnitude > (arenaRadius - avoidDistance))
        {
            return (-pos).normalized * maxVelocity;
        }
        else
        {
            return Vector3.zero;
        }
        
    }

    private GameObject FindClosestPlayer()
    {
        var playerManagers = GameManager.Instance.playerManagers;

        float shortestDistance = 10000.0f;
        GameObject closestPlayer = null;

        foreach (GameObject manager in playerManagers)
        {
            PlayerManager testManager = manager.GetComponent<PlayerManager>();
            if (testManager.playerID == this.playerID) { continue; }

            GameObject testPlayer = manager.GetComponent<PlayerManager>().myPlayer;
            if (testManager.inShadowRealm != inShadowRealm) { continue; }

            float dist = (this.transform.position - testPlayer.transform.position).magnitude;

            if (dist < shortestDistance)
            {
                shortestDistance = dist;
                closestPlayer = testPlayer;
            }
        }

        return closestPlayer;
    }
}

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

    private PlayerControllerRigidbody myController;
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
        myController = GetComponent<PlayerControllerRigidbody>();
        rigidBody = GetComponent<Rigidbody>();
        changeTargetTimer = changeTargetCheck;
        attackTimer = Random.Range(0.5f, 1.5f);
    }

    private void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (myController.IsDisabled()) { return; }

        steeringForce = Vector3.zero;

        changeTargetTimer -= Time.fixedDeltaTime;
        attackTimer -= Time.fixedDeltaTime;

        // Check for a new target
        if (changeTargetTimer < 0.0f)
        {
            targetPlayer = FindClosestPlayer();
            changeTargetTimer = changeTargetCheck;
        }

        // If we have a target player
        if (targetPlayer)
        {
            // If out of range, seek
            if ((this.transform.position - targetPlayer.transform.position).magnitude > attackDistance)
            {
                steeringForce += Seek(targetPlayer.transform.position);
                myController.SetLook(targetPlayer.transform.position - this.transform.position);
            }
            else if (attackTimer < 0.0f)
            {
                // If not charging or firing, start charging
                if (!myController.IsCharging() && !myController.IsFiring())
                {
                    myController.StartCharging();
                    attackTimer = Random.Range(0.5f, 1.5f);
                }
                // If not firing and are charging, fire
                else if (!myController.IsFiring() && myController.IsCharging())
                {
                    myController.SetLook(targetPlayer.transform.position - this.transform.position);
                    myController.FireProjectile();
                }

            }
            else
            {
                myController.SetLook(targetPlayer.transform.position - this.transform.position);
            }
        }
        else
        {
            myController.SetLook(rigidBody.velocity);
        }

        steeringForce += Containment();

        steeringForce = Vector3.ClampMagnitude(steeringForce, maxSteeringForce);

        currentForce = Vector3.ClampMagnitude(currentForce + steeringForce, maxVelocity);
        myController.Move(currentForce);
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

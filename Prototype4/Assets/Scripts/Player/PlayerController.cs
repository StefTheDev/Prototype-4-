using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(AudioSource))]
public class PlayerController : MonoBehaviour
{
    public GameObject airBlastPrefab, inhalePrefab;

    public float chargeTime = 1.0f;
    
    public float currentCharge = 0.0f;
    public bool disabled = false;

    public float brakingForceMultiplier = 1.0f;

    public AudioClip exhaleSound;
    public AudioClip inhaleSound;

    private Rigidbody rigidBody;
    private Vector3 moveVector = Vector3.zero;
    private bool isCharging = false;
    private AudioSource audioSource;
    private bool applyBrakingForce = false;

    private float maxSpeed = 10.0f;

    private Animator animator;
    private GameObject inhale;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        animator = GetComponentInChildren<Animator>();
    }

    public void Move(Vector3 _moveDirection)
    {
        moveVector = _moveDirection;
    }

    public void Disable()
    {
        disabled = true;
        rigidBody.velocity = Vector3.zero;
    }

    public void Enable()
    {
        disabled = false;
    }

    private void Update()
    {
        if (disabled) { return; }

        if (isCharging)
        {
            currentCharge = Mathf.Clamp(currentCharge + (Time.deltaTime / chargeTime), 0.0f, 1.0f);
        }

        if (animator)
        {
            animator.SetBool("Inhaling", isCharging);
            animator.SetFloat("Speed", rigidBody.velocity.magnitude);
        }

        if(inhale != null) inhale.transform.position = this.transform.position;
        
    }

    public void StartCharging()
    {
        if (disabled) { return; }

        isCharging = true;
        currentCharge = 0.0f;

        inhale = Instantiate(inhalePrefab, this.transform.position, this.transform.rotation, null);

        audioSource.PlayOneShot(inhaleSound);
    }

    //public void FireProjectile()
    //{
    //    if (disabled) { return; }

    //    isCharging = false;

    //    Player player = GetComponent<Player>();

    //    var battlecry = GetComponent<BattlecryPowerupHolder>();
    //    if (battlecry && battlecry.hasPowerup)
    //    {
    //        for (float angle = 0.0f; angle < 360.0f; angle += 45.001f)
    //        {
    //            Vector3 localDir = Quaternion.Euler(0, angle, 0) * Vector3.forward;
    //            Vector3 direction = transform.TransformDirection(localDir);
    //            InstantiateProjectile(player, currentCharge, direction);
    //        }

    //        currentCharge = 0.0f;

    //        audioSource.Stop();
    //        audioSource.PlayOneShot(exhaleSound);
    //    }
    //    else
    //    {
    //        // Fire projectile
    //        InstantiateProjectile(player, currentCharge, transform.forward);

    //        currentCharge = 0.0f;

    //        audioSource.Stop();
    //        audioSource.PlayOneShot(exhaleSound);
    //    }
    //}

    private void FixedUpdate()
    {
        if (disabled) { return; }

        float moveModifier = 1.0f;
        if (isCharging) { moveModifier = 0.5f; }


        // If the players new velocity would be over the speed cap, don't add force
        if ((rigidBody.velocity + moveVector.normalized).magnitude < (maxSpeed + 1))
        {
            rigidBody.AddForce(moveVector * moveModifier);
        }

        if (moveVector != Vector3.zero)
        {
            transform.forward = moveVector;
        }
        // Apply braking force if there are no inputs
        if (moveVector == Vector3.zero || applyBrakingForce)
        {
            rigidBody.AddForce(-rigidBody.velocity * brakingForceMultiplier);
        }

        applyBrakingForce = false;
    }

    public void SetLook(Vector3 _lookDir)
    {
        _lookDir.y = 0.0f;
        transform.forward = _lookDir;
    }

    public void ApplyBrakingForce()
    {
        applyBrakingForce = true;
    }
}

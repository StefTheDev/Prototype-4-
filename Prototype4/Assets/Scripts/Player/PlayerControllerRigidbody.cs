using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(AudioSource))]
public class PlayerControllerRigidbody : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveForce = 10.0f;
    [SerializeField] private float inhalingMoveSpeedModifier = 0.5f;
    [SerializeField] private float airbourneDrag = 0.1f;
    [SerializeField] private float movingDrag = 0.5f;
    [SerializeField] private float stationaryDrag = 0.9f;
    [SerializeField] private float groundCheckDist = 0.1f;

    private float maxSpeed = 10.0f;
    private float dragCoefficient = 10.0f;
    private float currentDrag;
    private float moveSpeedModifier = 1.0f;
    private bool isCrouching = false;
    private bool isDisabled = false;
    private bool isGrounded = false;
    private float initialGroundCheckDist;

    [Header("Shout")]
    [SerializeField] private float chargeTime = 1.0f;

    public float currentCharge = 0.0f;
    public bool isCharging = false;

    [Header("References")]
    [SerializeField] private GameObject airBlastPrefab;
    [SerializeField] private GameObject inhalePrefab;
    [SerializeField] private AudioClip exhaleSound;
    [SerializeField] private AudioClip inhaleSound;

    private Rigidbody rigidBody;
    private Animator animator;
    private AudioSource audioSource;
    private GameObject inhale;
    private Player playerComp;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
        // Debug.Assert(rigidBody);
        animator = GetComponentInChildren<Animator>();
        audioSource = GetComponent<AudioSource>();
        playerComp = GetComponent<Player>();
        initialGroundCheckDist = groundCheckDist;
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        if (isDisabled) { return; }

        if (isCharging)
        {
            currentCharge = Mathf.Clamp(currentCharge + (Time.deltaTime / chargeTime), 0.0f, 1.0f);
        }

        if (inhale != null) inhale.transform.position = this.transform.position;
    }

    // Call in fixed update
    public void Move(Vector3 move, bool crouch)
    {
        if (move.magnitude > 1.0f) move.Normalize();

        CheckGrounded();

        moveSpeedModifier = (isCharging) ? inhalingMoveSpeedModifier : 1.0f;
        isCrouching = crouch;

        PlayerMovement(move);

        UpdateAnimator();

    }

    private void PlayerMovement(Vector3 move)
    {
        Vector3 moveVec = move.normalized * moveForce * Time.fixedDeltaTime * moveSpeedModifier;
        if (!isGrounded) { moveVec *= 0.1f; }

        rigidBody.AddForce(moveVec, ForceMode.Impulse);

        ApplyDrag((move == Vector3.zero));

        CapSpeed();

        // if (move != Vector3.zero) transform.forward = move;
    }

    private void UpdateAnimator()
    {

    }

    private void ApplyDrag(bool moveInputs)
    {
        currentDrag = moveInputs ? movingDrag : stationaryDrag;
        if (!isGrounded) { currentDrag = airbourneDrag; }

        var currentVelocity = rigidBody.velocity;
        currentVelocity.y = 0.0f;

        rigidBody.AddForce(-currentVelocity * currentDrag * dragCoefficient * Time.fixedDeltaTime, ForceMode.Impulse);
    }

    private void CapSpeed()
    {

    }

    public void StartCharging()
    {
        if (isDisabled) { return; }

        isCharging = true;
        currentCharge = 0.0f;

        inhale = Instantiate(inhalePrefab, this.transform.position, this.transform.rotation, null);
        GameObject.Destroy(inhale, 1.0f);

        audioSource.PlayOneShot(inhaleSound);
    }

    public void FireProjectile()
    {
        if (isDisabled) { return; }

        isCharging = false;

        // Fire projectile
        var airBlast = Instantiate(airBlastPrefab, this.transform.position, Quaternion.identity, null);
        airBlast.GetComponent<AirBlast>().Launch(this.transform.forward, currentCharge, playerComp.GetPlayerID());
        if (playerComp.inShadowRealm)
        {
            airBlast.layer = LayerMask.NameToLayer("Shadow Realm");
        }

        currentCharge = 0.0f;

        audioSource.Stop();
        audioSource.PlayOneShot(exhaleSound);
    }

    public void SetLook(Vector3 lookDir)
    {
        lookDir.y = 0.0f;
        transform.forward = lookDir;
    }

    public void SetDisabled(bool disabled)
    {
        isDisabled = disabled;

        if (isDisabled)
        {
            rigidBody.velocity = Vector3.zero;
        }
    }

    public bool IsDisabled()
    {
        return isDisabled;
    }

    private void CheckGrounded()
    {
        RaycastHit hitInfo;

        if (Physics.Raycast(this.transform.position + (Vector3.up * 0.1f), Vector3.down, out hitInfo, groundCheckDist))
        {
            isGrounded = true;
            groundCheckDist = rigidBody.velocity.y < 0 ? initialGroundCheckDist : 0.1f;
        }
        else
        {
            isGrounded = false;
            groundCheckDist = initialGroundCheckDist;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Debug.DrawLine(this.transform.position + (Vector3.up * 0.1f), this.transform.position + (Vector3.up * 0.1f) + (Vector3.down * groundCheckDist));
    }
}

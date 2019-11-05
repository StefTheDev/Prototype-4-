using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(AudioSource))]
public class PlayerControllerRigidbody : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveForce = 10.0f;
    [SerializeField] private float inhalingMoveSpeedModifier = 0.5f;
    [SerializeField] private float airborneDrag = 0.1f;
    [SerializeField] private float movingDrag = 0.5f;
    [SerializeField] private float stationaryDrag = 0.9f;
    [SerializeField] private float groundCheckDist = 0.1f;
    [SerializeField] private float maxCrouchDuration = 0.5f;
    [SerializeField] private float crouchCooldown = 0.5f;

    private float crouchingTime = 0.0f;
    private float crouchCooldownTimer = 0.0f;
    
    private float dragCoefficient = 10.0f;
    private float currentDrag;
    private float moveSpeedModifier = 1.0f;
    private bool isCrouching = false;
    private bool isDisabled = false;
    public bool isGrounded = false;
    private float initialGroundCheckDist;
    private float startingMass;
    public bool isFiring = false;

    [Header("Shout")]
    [SerializeField] private float chargeTime = 1.0f;

    public float currentCharge = 0.0f;
    public bool isCharging = false;

    [Header("References")]
    [SerializeField] private GameObject airBlastPrefab;
    [SerializeField] private GameObject inhalePrefab;
    [SerializeField] private AudioClip exhaleSound;
    [SerializeField] private AudioClip inhaleSound;

    public GameObject ghostParticles;

    private Rigidbody rigidBody;
    private Animator animator;
    private AudioSource audioSource;
    private GameObject inhale;
    private Player playerComp;
    private SkinnedMeshRenderer meshRenderer;

    private float[] pitches = { 0.95f, 0.98f, 1.02f, 1.05f };

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
        audioSource = GetComponent<AudioSource>();
        playerComp = GetComponent<Player>();
        initialGroundCheckDist = groundCheckDist;
        startingMass = rigidBody.mass;
        meshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();

        var mats = meshRenderer.materials;
        mats[0] = new Material(mats[0]);
        meshRenderer.materials = mats;

        audioSource.pitch = pitches[playerComp.playerID];
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

        if (isCrouching)
        {
            UpdateCrouch();
        }

        crouchCooldownTimer -= Time.deltaTime;

        if (inhale != null) inhale.transform.position = this.transform.position;
    }

    // Call in fixed update
    public void Move(Vector3 move)
    {
        UpdateAnimator();
        if (isDisabled) { return; }

        if (move.magnitude > 1.0f) move.Normalize();

        CheckGrounded();

        moveSpeedModifier = (isCharging || isFiring) ? inhalingMoveSpeedModifier : 1.0f;

        PlayerMovement(move);

        // UpdateAnimator();

        if (ghostParticles) { ghostParticles.transform.position = this.transform.position; }
    }

    private void PlayerMovement(Vector3 move)
    {
        Vector3 moveVec = move.normalized * moveForce * Time.fixedDeltaTime * moveSpeedModifier;
        if (!isGrounded) { moveVec *= 0.1f; }
        if (isCrouching) { moveVec = Vector3.zero; }

        rigidBody.AddForce(moveVec, ForceMode.Impulse);

        ApplyDrag((move == Vector3.zero));

        CapSpeed();
    }

    private void UpdateAnimator()
    {
        animator.SetBool("Inhaling", isCharging);
        animator.SetFloat("Speed", rigidBody.velocity.magnitude);

        meshRenderer.materials[0].SetFloat("_ChargeAmount", currentCharge / chargeTime);
    }

    private void ApplyDrag(bool moveInputs)
    {
        currentDrag = moveInputs ? movingDrag : stationaryDrag;
        if (!isGrounded) { currentDrag = airborneDrag; }
        if (isCrouching) { currentDrag *= 1.3f; }

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
        if (isFiring) { return; }

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
        isFiring = true;
        // transform.DOLocalMove(this.transform.position, 0.7f).OnComplete(OnShoutAnimEnd);
        var seq = DOTween.Sequence();
        seq.AppendInterval(0.7f).OnComplete(OnShoutAnimEnd);

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

    public void OnShoutAnimEnd()
    {
        isFiring = false;
    }

    public void StartCrouch()
    {
        if (crouchCooldownTimer > 0.0f)
        {
            return;
        }

        transform.DOKill();
        transform.DOScaleY(0.3f, 0.01f);

        rigidBody.mass = startingMass * 1.3f;

        isCrouching = true;
        crouchingTime = 0.0f;
    }

    private void UpdateCrouch()
    {
        crouchingTime += Time.deltaTime;

        if (crouchingTime >= maxCrouchDuration)
        {
            EndCrouch();
        }
    }

    public void EndCrouch()
    {
        isCrouching = false;
        crouchCooldownTimer = crouchCooldown;

        transform.DOKill();
        transform.DOScaleY(1.0f, 0.01f);

        rigidBody.mass = startingMass;
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

    public bool IsCrouching()
    {
        return isCrouching;
    }

    public bool IsFiring()
    {
        return isFiring;
    }

    public bool IsCharging()
    {
        return isCharging;
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

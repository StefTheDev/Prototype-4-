using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct ShotHitInfo
{
    public Vector3 point;
    public Vector3 normal;

    public ShotHitInfo(Vector3 point, Vector3 normal)
    {
        this.point = point;
        this.normal = normal;
    }
}

/*
 * The controller for the shield powerup's effects.
 * 
 * Attach to player with rigidbody.
 * Author: Elijah Shadbolt
*/
public class ShieldPowerup : MonoBehaviour
{
	public bool isPowerupActive => shieldCurrentHealth > 0.0f;
	public Rigidbody rigidBody { get; private set; }

	public float shieldStartHealth = 2.05f;
	public float shieldCurrentHealth { get; private set; } = 0.0f;

    [Range(0.0f, 1.0f)]
    public float resistance = 0.7f;

    public event Action onBeginEffects;
	public event Action<float> onHealthFractionChanged;
	public event Action onEndEffects;
    public event Action<ShotHitInfo> onHit;

    private AudioSource audioSource;

	private void Awake()
	{
		rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
	}

    private void Start()
    {
        GameManager.Instance.onGameEnded += EndEffects;
    }

    private void InvokeHealthFractionChanged()
	{
		onHealthFractionChanged?.Invoke(shieldCurrentHealth / shieldStartHealth);
	}

	public void BeginEffects()
	{
		this.shieldCurrentHealth = this.shieldStartHealth;
		onBeginEffects?.Invoke();
		onHealthFractionChanged?.Invoke(1.0f);
        AudioManager.Instance.PlaySound("ShieldPowerup");

        var battlecry = GetComponent<BattlecryPowerupHolder>();
        if (battlecry) battlecry.EndEffects();
    }

	public void EndEffects()
	{
		this.shieldCurrentHealth = 0.0f;
		onHealthFractionChanged?.Invoke(0.0f);
		onEndEffects?.Invoke();
    }

	public void ApplyAirBlast(ShotHitInfo hit, Vector3 force, float chargeAmount)
	{
		if (!isPowerupActive)
		{
			rigidBody.AddForce(force, ForceMode.Impulse);
		}
		else
		{
			rigidBody.AddForce(force * (1.0f - resistance), ForceMode.Impulse);

			float damage = chargeAmount;
			shieldCurrentHealth = Mathf.Clamp(shieldCurrentHealth - damage, 0.0f, shieldStartHealth);
			InvokeHealthFractionChanged();
			if (shieldCurrentHealth <= 0.0f)
			{
				onEndEffects?.Invoke();
                AudioManager.Instance.PlaySound("ShieldBreak", 2.0f);
            }
		}
        onHit?.Invoke(hit);
	}
}

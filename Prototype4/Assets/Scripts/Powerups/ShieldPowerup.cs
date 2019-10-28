using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

	public float shieldStartHealth = 1.0f;
	public float shieldCurrentHealth { get; private set; } = 0.0f;

	public event Action onBeginEffects;
	public event Action<float> onHealthFractionChanged;
	public event Action onEndEffects;

	private void Awake()
	{
		rigidBody = GetComponent<Rigidbody>();
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
	}

	public void EndEffects()
	{
		this.shieldCurrentHealth = 0.0f;
		onHealthFractionChanged?.Invoke(0.0f);
		onEndEffects?.Invoke();
	}

	public void ApplyAirBlast(Vector3 force)
	{
		if (!isPowerupActive)
		{
			rigidBody.AddForce(force);
		}
		else
		{
			float amount = 0.3f;
			rigidBody.AddForce(force * amount);

			float damage = force.magnitude * (1.0f - amount) * 0.0005f;
			Debug.Log("damage: " + damage);
			shieldCurrentHealth = Mathf.Clamp(shieldCurrentHealth - damage, 0.0f, shieldStartHealth);
			InvokeHealthFractionChanged();
			if (shieldCurrentHealth <= 0.0f)
			{
				onEndEffects?.Invoke();
			}
		}
	}
}

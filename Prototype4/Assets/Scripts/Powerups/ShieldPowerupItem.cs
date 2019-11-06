using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * The item that can be picked up by the player to activate the shield powerup.
 * 
 * Attach to trigger collider.
 * Author: Elijah Shadbolt
*/
public class ShieldPowerupItem : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
		var collector = other.GetComponentInParent<ShieldPowerup>();
		if (collector && collector.gameObject.layer == LayerMask.NameToLayer("Normal Realm"))
		{
			collector.BeginEffects();
			Destroy(gameObject);
		}
	}
}

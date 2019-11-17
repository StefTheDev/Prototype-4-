using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindwalkerPowerupItem : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        var collector = other.GetComponentInParent<WindwalkerPowerup>();
        if (collector && collector.gameObject.layer == LayerMask.NameToLayer("Normal Realm"))
        {
            collector.BeginEffects();
            Destroy(gameObject);
        }
    }
}

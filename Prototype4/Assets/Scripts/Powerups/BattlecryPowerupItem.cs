using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattlecryPowerupItem : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        var collector = other.GetComponentInParent<BattlecryPowerupHolder>();
        if (collector && collector.gameObject.layer == LayerMask.NameToLayer("Normal Realm"))
        {
            collector.hasPowerup = true;
            Destroy(gameObject);
        }
    }
}

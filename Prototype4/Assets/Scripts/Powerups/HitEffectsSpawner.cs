using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEffectsSpawner : MonoBehaviour
{
    public ShieldPowerup player;
    public GameObject prefab;

    private void Awake()
    {
        player.onHit += (args) => Instantiate(prefab, args.point, Quaternion.LookRotation(args.normal));
    }
}

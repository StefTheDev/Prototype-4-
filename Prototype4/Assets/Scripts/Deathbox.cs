using System;

using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class Deathbox : MonoBehaviour
{
#pragma warning disable CS0649
    [SerializeField, ReadOnly]
    private List<GameObject> vulnerable;

    [SerializeField]
    private SpawnPoints spawnPoints;
#pragma warning restore CS0649

    private void OnTriggerStay(Collider other)
    {
        if (vulnerable.Contains(other.gameObject))
        {
            spawnPoints.GetRandom().Teleport(other.gameObject.transform);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        var player = other.GetComponent<Player>();

        if (player)
        {
            GameManager.Instance.playerManagerObjects[player.GetPlayerID()].GetComponent<PlayerManager>().PlayerDeath();
        }
    }

    public List<GameObject> GetVulnerable()
    {
        return vulnerable;
    }
}

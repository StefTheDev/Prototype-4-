using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoints : MonoBehaviour
{
    public List<SpawnPoint> spawnPoints;

    public SpawnPoint GetRandom()
    {
        return spawnPoints[Random.Range(0, spawnPoints.Count)];
    }
}

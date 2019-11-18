using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelThrower : MonoBehaviour
{
    // public Rigidbody prefab;
    public GameObject barrelPrefab;

    public Transform target;
    public float range = 10.0f;
    public float arcMin = 0.0f;
    public float arcMax = 360.0f;
    public float angleMin = 35.0f;
    public float angleMax = 55.0f;
    public float force = 100.0f;

    public GameObject[] powerupItemPrefabs;

    public bool spawnOnStart = false;
    public int numToSpawn = 3;

    private void Start()
    {
        GameManager.Instance.onGameStarted += () => {
            if (spawnOnStart)
            {
                SpawnBarrels();
            }
        };
    }

    public void SpawnBarrels()
    {
        SpawnBarrels(numToSpawn);
    }

    public void SpawnBarrels(int num)
    {
        for (int i = 0; i < num; i++)
        {
            SpawnRandomBarrel();
        }
    }

    public void SpawnRandomBarrel()
    {
        SpawnBarrel(
            arc: Random.Range(arcMin, arcMax),
            angle: Random.Range(angleMin, angleMax),
            powerupItemPrefab: powerupItemPrefabs.Length > 0
            ? powerupItemPrefabs[Random.Range(0, powerupItemPrefabs.Length)]
            : null
            );
    }

    public void SpawnBarrel(float arc, float angle, GameObject powerupItemPrefab)
    {
        if (!target) { target = transform; }

        Quaternion arcRotation = Quaternion.Euler(0, arc, 0);
        Quaternion angleRotation = Quaternion.Euler(-angle, 0, 0);
        Quaternion localRotation = arcRotation * angleRotation;
        Quaternion worldRotation = target.rotation * localRotation;
        Vector3 localPosition = arcRotation * -Vector3.forward * range;
        Vector3 worldPosition = target.TransformPoint(localPosition);
        Vector3 forceVector = worldRotation * Vector3.forward * force;

        var newBarrel = Instantiate(barrelPrefab, worldPosition, worldRotation, transform);
        // var rb = Instantiate(prefab, worldPosition, worldRotation, transform);
        var rb = newBarrel.GetComponentInChildren<Rigidbody>();
        rb.AddForce(forceVector, ForceMode.Impulse);

        var barrel = newBarrel.GetComponent<Barrel>();
        if (!barrel) { Debug.LogError("barrel is null", this); }
        else
        {
            if (powerupItemPrefab)
            {
                barrel.powerupPrefab = powerupItemPrefab;
            }
        }
    }
}

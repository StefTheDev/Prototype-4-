using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogSpawner : MonoBehaviour
{
    public float spawnCooldown = 1.0f;

    private float spawnTimer = 0.0f;


    public GameObject logPrefab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        spawnTimer += Time.deltaTime;

        if (spawnTimer >= spawnCooldown)
        {
            spawnTimer -= spawnCooldown;

            float spawnDistance = Random.Range(0.0f, 9.0f);
            float spawnAngle = Random.Range(0.0f, 360.0f) * Mathf.Deg2Rad;
            float spawnHeight = Random.Range(50.0f, 100.0f);

            Vector3 spawnPos = new Vector3(spawnDistance * Mathf.Cos(spawnAngle), spawnHeight, spawnDistance * Mathf.Sin(spawnAngle));

            Instantiate(logPrefab, spawnPos, Quaternion.identity, null);
        }
    }
}

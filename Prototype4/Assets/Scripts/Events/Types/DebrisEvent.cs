using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebrisEvent : Event
{
    int numLogs = 3;
    public override void OnEnd()
    {

    }

    public override void OnStart()
    {
        for (int i = 0; i < numLogs; i++)
        {
            float spawnDistance = Random.Range(0.0f, 9.0f);
            float spawnAngle = Random.Range(0.0f, 360.0f) * Mathf.Deg2Rad;
            float spawnHeight = Random.Range(50.0f, 100.0f);

            Vector3 spawnPos = new Vector3(spawnDistance * Mathf.Cos(spawnAngle), spawnHeight, spawnDistance * Mathf.Sin(spawnAngle));

            Instantiate(ReferenceManager.Instance.logPrefab, spawnPos, Quaternion.identity, null);
        }
    }
}

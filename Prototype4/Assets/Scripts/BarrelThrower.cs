using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelThrower : MonoBehaviour
{
    public Rigidbody prefab;

    public Transform target;
    public float range = 10.0f;
    public float arcMin = 0.0f;
    public float arcMax = 360.0f;
    public float angleMin = 35.0f;
    public float angleMax = 55.0f;
    public float force = 100.0f;

    private void Start()
    {
        GameManager.Instance.onGameStarted += SpawnLots;
    }

    private void SpawnLots()
    {
        SpawnBarrel();
        SpawnBarrel();
        SpawnBarrel();
    }

    private void SpawnBarrel()
    {
        SpawnBarrel(arc: Random.Range(arcMin, arcMax), angle: Random.Range(angleMin, angleMax));
    }

    private void SpawnBarrel(float arc, float angle)
    {
        Quaternion arcRotation = Quaternion.Euler(0, arc, 0);
        Quaternion angleRotation = Quaternion.Euler(-angle, 0, 0);
        Quaternion localRotation = arcRotation * angleRotation;
        Quaternion worldRotation = target.rotation * localRotation;
        Vector3 localPosition = arcRotation * -Vector3.forward * range;
        Vector3 worldPosition = target.TransformPoint(localPosition);
        Vector3 forceVector = worldRotation * Vector3.forward * force;
        var rb = Instantiate(prefab, worldPosition, worldRotation, transform);
        rb.AddForce(forceVector, ForceMode.Impulse);
        Debug.DrawRay(worldPosition, forceVector, Color.red, 5);
    }
}

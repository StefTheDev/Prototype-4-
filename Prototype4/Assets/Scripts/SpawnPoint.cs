using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    private bool occupied = false;

    public void Teleport(Transform targetTransform)
    {
        targetTransform.position = transform.position;
        occupied = true;
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(transform.position, Vector3.one);

        Gizmos.color = Color.magenta;
        Gizmos.DrawCube(transform.position, new Vector3(0.5f, 0.5f, 0.5f));
    }

    public void SetOccupied(bool occupied)
    {
        this.occupied = occupied;
    }

    public bool IsOccupied()
    {
        return occupied;
    }
}

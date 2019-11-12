using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelThrowerManager : MonoBehaviour
{
    public float seconds = 30;
    public BarrelThrower thrower;

    private void Awake()
    {
        if (!thrower) { thrower = GetComponent<BarrelThrower>(); }
    }

    private void Start()
    {
        GameManager.Instance.onGameStarted += () => StartCoroutine(SpawnBarrelsLoop());
    }

    IEnumerator SpawnBarrelsLoop()
    {
        while (true)
        {
            thrower.SpawnBarrels();
            yield return new WaitForSeconds(seconds);
        }
    }
}

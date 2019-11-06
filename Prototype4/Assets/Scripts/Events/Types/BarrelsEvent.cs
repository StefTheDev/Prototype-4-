using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BarrelsEvent : Event
{
    private GameManager gameManager = GameManager.Instance;

    public override void OnEnd()
    {

    }

    public override void OnStart()
    {
        var thrower = GetComponent<BarrelThrower>();
        if (thrower) thrower.SpawnBarrels();
        else { Debug.LogError("BarrelThrower is null", this); }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class StormEvent : Event
{
    //Storm Event

    //1. Lightning
    //a. Indicator circle to display
    //b. Once the indicator finished, hit lightning at position
    //2. Rain 
    //a. Will start before the lightning maybe 5-10 seconds
    //b. Slowly fades once it reaches close to the end of the event
    //3. Fog
    //a. Will fall before the rain
    //b. Also fades away but before the rain cancels out. 

    //[SerializeField] private GameObject lightning, rain;

    /*private ParticleSystem.Particle[] particles;
    public bool includeChildren = true;
    */



    public override void OnStart()
    {
        Debug.Log("Storm Event Started.");

        var seq = DOTween.Sequence();

        // Create lightning strikes
        for (int i = 0; i < 5; i++)
        {
            float delay = Random.Range(2.0f, 4.0f);
            seq.AppendInterval(delay).AppendCallback(() => LightningStrike());
        }
        //lightning.SetActive(true);
    }

    public override void OnEnd()
    {
        Debug.Log("Storm Event Ended.");
        //lightning.SetActive(false);
    }

    private void OnParticleCollision(GameObject other)
    {
        if(other.GetType() == typeof(Player))
        {
            Debug.Log("Player has been hit");
        }
    }

    private void LightningStrike()
    {
        // Choose random position in arena
        float angle = Random.Range(0.0f, 360.0f);
        float dist = Random.Range(0.0f, 9.0f);

        Vector3 spawnPos = new Vector3(dist * Mathf.Cos(angle), 0.0f, dist * Mathf.Sin(angle));
        var strike = Instantiate(ReferenceManager.Instance.lightningStrike, spawnPos, Quaternion.identity, null);
        GameObject.Destroy(strike, 10.0f);
        // Debug.Log("LIGHTNING STRIKE");
    }
    
}

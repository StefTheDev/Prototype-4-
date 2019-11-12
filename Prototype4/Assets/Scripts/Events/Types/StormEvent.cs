using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StormEvent : Event
{
#pragma warning disable CS0649
    [SerializeField] private GameObject lightning, rain;
    [SerializeField] private ParticleSystem indicator;
#pragma warning restore CS0649

    private ParticleSystem.Particle[] particles;

    public bool includeChildren = true;

    public override void OnStart()
    {
        Debug.Log("Storm Event Started.");
        lightning.SetActive(true);
    }

    public override void OnEnd()
    {
        Debug.Log("Storm Event Ended.");
        lightning.SetActive(false);
    }

    private void OnParticleCollision(GameObject other)
    {
        if(other.GetType() == typeof(Player))
        {
            Debug.Log("Player has been hit");
        }
    }
}

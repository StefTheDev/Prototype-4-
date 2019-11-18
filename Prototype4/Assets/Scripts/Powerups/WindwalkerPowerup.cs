using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindwalkerPowerup : MonoBehaviour
{
    public new ParticleSystem particleSystem;
    public float duration = 5.0f;
    
    private float m_timer = 0.0f;
    public bool hasPowerup
    {
        get { return m_timer > 0.0f; }
    }

    private void Start()
    {
        GameManager.Instance.onGameEnded += EndEffects;
        EndEffects();
    }

    public void BeginEffects()
    {
        m_timer = duration;
        if (particleSystem)
        {
            var emitter = particleSystem.emission; // have to store in local variable for some reason
            emitter.enabled = true;
        }

        AudioManager.Instance.PlaySound("PowerupPickup");
        var shield = GetComponent<ShieldPowerup>();
        if (shield) shield.EndEffects();
        var battlecry = GetComponent<BattlecryPowerupHolder>();
        if (battlecry) battlecry.EndEffects();
    }

    public void EndEffects()
    {
        m_timer = 0.0f;
        if (particleSystem)
        {
            var emitter = particleSystem.emission; // have to store in local variable for some reason
            emitter.enabled = false;
        }
    }

    private void Update()
    {
        if (m_timer > 0.0f)
        {
            m_timer -= Time.deltaTime;
            if (m_timer <= 0.0f)
            {
                EndEffects();
            }
        }
    }
}

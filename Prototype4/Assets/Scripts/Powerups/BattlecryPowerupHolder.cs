using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattlecryPowerupHolder : MonoBehaviour
{
    public GameObject visuals;

    [SerializeField]
    private bool m_hasPowerup = false;
    public bool hasPowerup
    {
        get { return m_hasPowerup; }
        private set
        {
            m_hasPowerup = value;
            if (visuals) visuals.SetActive(value);
        }
    }

    public void BeginEffects()
    {
        hasPowerup = true;

        AudioManager.Instance.PlaySound("PowerupPickup");
        var shield = GetComponent<ShieldPowerup>();
        if (shield) shield.EndEffects();
        var windwalker = GetComponent<WindwalkerPowerup>();
        if (windwalker) windwalker.EndEffects();
    }

    public void EndEffects()
    {
        hasPowerup = false;
    }

    private void Start()
    {
        GameManager.Instance.onGameEnded += EndEffects;
        hasPowerup = m_hasPowerup;
    }
}

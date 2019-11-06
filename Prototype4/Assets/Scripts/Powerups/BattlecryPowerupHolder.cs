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
        set
        {
            m_hasPowerup = value;
            if (visuals) visuals.SetActive(value);
        }
    }

    private void Start()
    {
        hasPowerup = m_hasPowerup;
    }
}

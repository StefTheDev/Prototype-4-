using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    private TMP_Text label;

    [SerializeField]
    private float time = 60.0f;

    private void Start()
    {
        label = GetComponent<TMP_Text>();
    }

    private void FixedUpdate()
    {
        time -= Time.deltaTime;
        label.text = time.ToString("#");
    }

    public float GetTime()
    {
        return time;
    }

}

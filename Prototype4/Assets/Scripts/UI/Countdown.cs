using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Countdown : MonoBehaviour
{
    public TextMeshProUGUI numText;
    public int countdownStart = 5;

    private int currentCounter;

    private void OnDestroy()
    {
        AudioManager.Instance.PlaySound("RoundStart");
        GameManager.Instance.StartGame();
        // AudioManager.Instance.PlaySound("GameMusic");
    }

    private void Awake()
    {
        Destroy(this.gameObject, (float)countdownStart);
        currentCounter = countdownStart;
        numText.text = currentCounter.ToString();
    }

    public void CountDown()
    {
        currentCounter--;
        numText.text = currentCounter.ToString();
    }

    public void TryPlayMusic()
    {
        if (currentCounter == 1)
        {
            
        }
    }

    public void PlaySoundEffect()
    {
        AudioManager.Instance.PlaySound("CountdownDrum");

        //if (currentCounter > 1)
        //{
            
        //}
        //else if (currentCounter == 1)
        //{
        //    //AudioManager.Instance.PlaySound("RoundStart");            
        //}
    }
}

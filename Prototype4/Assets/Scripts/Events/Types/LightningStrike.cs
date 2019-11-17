using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LightningStrike : MonoBehaviour
{
    private float delay = 1.0f;
    private float radius = 2.0f;
    private float strikeForce = 50.0f;
    public GameObject bolt;
    public GameObject impact;

    private void Start()
    {
        var seq = DOTween.Sequence();
        seq.AppendInterval(delay).OnComplete(ActivateBolt);

        var audioSeq = DOTween.Sequence();
        audioSeq.AppendInterval(0.2f).AppendCallback(() => AudioManager.Instance.PlaySound("LightningStrike", 1.5f));
    }

    private void Knockback()
    {
        var managers = GameManager.Instance.playerManagers;

        for (int i = 0; i < managers.Count; i++)
        {
            Player player = managers[i].myPlayer.GetComponent<Player>();

            Vector3 toPlayer = player.transform.position - this.transform.position;
            toPlayer.y = 0.0f;

            float mag = toPlayer.magnitude;

            // Struck by lightning
            if (mag <= radius)
            {
                float severity = 1.0f - (mag / radius);
                player.GetComponent<Rigidbody>().AddForce(toPlayer.normalized * strikeForce * severity, ForceMode.Impulse);
                Debug.Log("STRUCK");
            }
        }
    }

    private void ActivateBolt()
    {
        // bolt.SetActive(true);
        // impact.SetActive(true);
        Knockback();
    }
}

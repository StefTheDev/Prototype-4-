using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BillboardSprite : MonoBehaviour
{
    public GameObject billboardObject;
    private Camera gameCamera;
    public float amplitude = 0.5f;
    public float duration = 2;
    private float phasor = 0;

    private void Start()
    {
        gameCamera = Camera.main;

        DOTween.To(
            () => phasor,
            v => {
                phasor = v;
                transform.localPosition = new Vector3(0, amplitude * Mathf.Sin(phasor * Mathf.Deg2Rad), 0);
            },
            360,
            duration
            )
            .SetEase(Ease.Linear)
            .OnComplete(() => phasor = 0)
            .SetLoops(-1);
    }

    // Update is called once per frame
    void Update()
    {
        billboardObject.transform.LookAt(gameCamera.transform.position, Vector3.up);
    }
}

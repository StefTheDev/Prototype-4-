using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BillboardSprite : MonoBehaviour
{
    public GameObject billboardObject;
    private Camera gameCamera;


    private void Start()
    {
        gameCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        billboardObject.transform.LookAt(gameCamera.transform.position, Vector3.up);
    }
}

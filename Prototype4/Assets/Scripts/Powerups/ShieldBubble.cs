using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ShieldBubble : MonoBehaviour
{
	public Transform particles;

	private void Start()
	{
		particles.localScale = new Vector3(0.01f, 0.01f, 0.01f);
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Q)) // DEBUG
		{
			particles.DOScale(new Vector3(1, 1, 1), 1).SetEase(Ease.OutElastic);
		}
	}

	private void LateUpdate()
	{
		transform.LookAt(Camera.main.transform);
	}
}

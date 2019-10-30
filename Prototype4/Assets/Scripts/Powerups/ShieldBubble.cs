using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/*
 * The visual effects particle system for the shield powerup.
 * 
 * Attach to player or one of its child transforms.
 * Author: Elijah Shadbolt
 */
public class ShieldBubble : MonoBehaviour
{
	public ShieldPowerup controller;
	public Transform scaleRoot; // particle system transform.
	public ParticleSystem particles;
	public Gradient colorWrtHealthFraction;

	private static readonly Vector3 minScale = new Vector3(0.001f, 0.001f, 0.001f);

	private void Awake()
	{
		scaleRoot.localScale = minScale;

		controller.onBeginEffects += OnBeginEffects;
		controller.onEndEffects += OnEndEffects;
		controller.onHealthFractionChanged += OnHealthFractionChanged;
	}

	private void OnBeginEffects()
	{
		scaleRoot.DOScale(new Vector3(1, 1, 1), 1).SetEase(Ease.OutElastic);
	}

	private void OnEndEffects()
	{
		scaleRoot.DOScale(minScale, 1).SetEase(Ease.OutElastic);
	}

	private void OnHealthFractionChanged(float fraction)
	{
        //var main = particles.main;
        //var s = main.startColor;
        //var c = s.color;
        //c.a = fraction;
        //s.color = c;
        //main.startColor = s;

        var col = particles.colorOverLifetime;
		var grad = col.color;
		var keys = grad.gradient.colorKeys;
        Color desiredColor = colorWrtHealthFraction.Evaluate(fraction);
		for (int i = 0; i < keys.Length; ++i)
		{
			keys[i].color = desiredColor;
		}
		grad.gradient.colorKeys = keys;

        //var alphaKeys = grad.gradient.alphaKeys;
        //if (fraction > 0.0f)
        //{
        //    alphaKeys[1].alpha = 0.25f;
        //    alphaKeys[2].alpha = 1.0f;
        //}
        //else
        //{
        //    alphaKeys[1].alpha = 0.0f;
        //    alphaKeys[2].alpha = 0.0f;
        //}
        //grad.gradient.alphaKeys = alphaKeys;

		col.color = grad;
	}

	private void LateUpdate()
	{
		transform.LookAt(Camera.main.transform);
	}
}

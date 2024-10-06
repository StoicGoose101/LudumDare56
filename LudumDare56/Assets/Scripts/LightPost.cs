using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using DG.Tweening;

public class LightPost : MonoBehaviour
{

	public float noiseScale = 1f;
	public float placementThreshold = 0.5f;
	public float prefabRandomValue = 0.5f;

	private float ActivatedTimer;
	public bool Active = false;
	public float ActivatedTime = 10;
	public float Intensity = 10;

	private Light2D light;
	private Attractor attractor;
	public AudioClip LightClip;
	private AudioSource audioSource;

	void Start()
	{
		ActivatedTimer = ActivatedTime;
		light = GetComponent<Light2D>();
		attractor = GetComponent<Attractor>();
		audioSource = GetComponent<AudioSource>();
	}

	void Update()
	{

		if (Input.GetKeyDown(KeyCode.Space))
		{
			Active = !Active;
			attractor.Active = Active;

			PlayLightAnim(Active);

			if (audioSource)
			{
				//audioSource.pitch = originalPitch + Mathf.PerlinNoise1D(timeMult * Time.time) * pitchScale - pitchScale;
				audioSource.PlayOneShot(LightClip, 1.0f);
			}
		}
	}

	public void PlayLightAnim(bool fadeIn)
	{
		float valFloat = 0f;
		float intensityTarget = Intensity;
		float fadeSpeed = 1.0f;

		if (!fadeIn)
		{
			valFloat = light.intensity;
			intensityTarget = 0;
		}

		TweenParams inParms = new TweenParams().SetEase(Ease.InOutBounce);
		Sequence s = DOTween.Sequence();
		s.Append(DOTween.To(() => valFloat, x => light.intensity = x, intensityTarget, fadeSpeed).SetAs(inParms));
	}

	public void StopSound()
	{
		audioSource = GetComponent<AudioSource>();
		audioSource.Stop();
	}
}

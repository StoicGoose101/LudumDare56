using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class CreatireBehaviour : MonoBehaviour
{
	public float time = 1;
	
	public float startScale = 1;
	public float endScaleY = 2;
	public float endScaleX = 2;

	public Color scaredColor;
	public float rotationScale = 1.0f;
	public float timeMult = 1.0f;

	public float shakeStrength = 1.0f;
	public int shakeVibrato = 10;
	public float shakeRandomness = 1.0f;

	private Color OriginalColor;
	private Vector3 OriginalPosition;

	Quaternion originalRotation;

	public bool IsGlowing = false;

	public Attractor[] Attractors;
	private AudioSource audioSource;
	private float originalPitch;

	public AudioClip SpawnClip;
	public AudioClip DeathClip;
	public float pitchScale = 1.0f;

	public float StartDelay = 0.4f;

    public float food = 100;
	public float warmth = 100;

	public float eatRate = 4;
	public float heatRate = 4;

	public float noiseSpeedMult = 2f;
	float speedModifier = 1;

	bool isDead = false;
	float deleteTimer;
	public float deleteTime = 5;

	void Start()
    {
        DOTween.Init(false, true, LogBehaviour.ErrorsOnly);
        originalRotation = transform.localRotation;

		//PlayIntro();

		PlayScaling();

		SpriteRenderer renderer = GetComponent<SpriteRenderer>();
		if (renderer)
		{
			Material nMaterial = renderer.material;
			OriginalColor = nMaterial.color;
		}
		OriginalPosition = transform.position;

		//PlaySpawnAnimation();

		audioSource = GetComponent<AudioSource>();
		if (audioSource)
			originalPitch = audioSource.pitch;

		Attractors = FindObjectsOfType<Attractor>();
		isDead = false;
		deleteTimer = deleteTimer;
	}

	public void PlayIntro()
	{
		SpriteRenderer renderer = GetComponent<SpriteRenderer>();
		Material nMaterial = renderer.material;
		Color addColor = new Color(10.5f, 10.5f, 10.0f, 1.0f);

		Sequence mySequence = DOTween.Sequence();
		TweenParams inParms = new TweenParams().SetEase(Ease.InOutBack);
		TweenParams inParms2 = new TweenParams().SetEase(Ease.OutCubic);

		Vector2 axis = new Vector3(1, 1, 0);

		mySequence.PrependInterval(2.0f);
		mySequence.Append(transform.DOScaleY(1, 0.2f).SetAs(inParms));
	}

	public void PlayScaling()
	{
		TweenParams inParms = new TweenParams().SetEase(Ease.InSine);
		TweenParams outParms = new TweenParams().SetEase(Ease.InOutSine);

		Sequence mySequence = DOTween.Sequence();

		float perlinNoise = Mathf.PerlinNoise(transform.position.x, transform.position.y);
		time *= 4.0f * perlinNoise;
		endScaleY *= perlinNoise * 0.2f + 0.9f;
		endScaleX *= perlinNoise * 0.2f + 0.9f;

		mySequence.Join(transform.DOScaleY(endScaleY, time).SetAs(inParms));
		mySequence.Join(transform.DOScaleX(endScaleX, time).SetAs(inParms));

		mySequence.Join(transform.DOShakeRotation(endScaleX, time).SetAs(inParms));

		SpriteRenderer renderer = GetComponent<SpriteRenderer>();
		Material nMaterial = renderer.material;

		Color color = nMaterial.color;
		Color addColor = new Color(0.0f, 0.1f, 0.4f, 0.8f);
		if (IsGlowing)
			addColor = new Color(1.0f, 1.0f, 0.0f, 0.8f);

		mySequence.Join(nMaterial.DOColor(addColor, time).SetAs(inParms));
		mySequence.SetLoops(-1, LoopType.Yoyo);
    }

    void Update()
    {
        UpdateSpeed();

		food -= Time.deltaTime;
		warmth -= Time.deltaTime;

		if (!isDead && (food <= 0 || warmth <= 0 || warmth >= 60))
        {
            isDead = true;
			speedModifier = 0;
		
			Rigidbody2D rigidBody = GetComponent<Rigidbody2D>();
			rigidBody.gravityScale = 5.0f;

			deleteTimer = deleteTime;
			//Destroy(this.gameObject);


			if (audioSource)
			{
				//audioSource.pitch = originalPitch + Mathf.PerlinNoise1D(timeMult * Time.time) * pitchScale - pitchScale;
				audioSource.PlayOneShot(SpawnClip, 0.1f);
			}
		}

		if (food >= 60)
			speedModifier = 0.5f;
		else
			speedModifier = 1.5f;

		if (isDead)
		{
			deleteTimer -= Time.deltaTime;
			if (deleteTimer <= 0)
				Destroy(this.gameObject);
		}
	}

	void UpdateSpeed()
	{
		float timeMult = 5f;
		//float noiseScale = 3f;

		float perlinNoiseX = Mathf.PerlinNoise(transform.position.x + Time.fixedTime * timeMult, transform.position.y) - 0.5f;
		float perlinNoiseY = Mathf.PerlinNoise(transform.position.x, transform.position.y + Time.fixedTime * timeMult) - 0.5f;

		
		Vector2 noiseSpeed = new Vector2(perlinNoiseX, perlinNoiseY) * noiseSpeedMult;

		//float attractionVal = 1.1f;
		Vector2 totalAttractionSpeed = new Vector2(0,0);

		foreach (var attractor in Attractors)
		{
			float attractionVal = attractor.AttractStrength;

			if (!attractor.Active)
				attractionVal = 0;

			if (Vector2.Distance(attractor.transform.position, transform.position) <= 1.5f)
			{
				attractionVal = 0.1f;
				speedModifier = 0.1f;
				if (attractor.IsFood)
                {
					food += Time.deltaTime * eatRate;
                }
                else
                {
					warmth += Time.deltaTime * heatRate;
                }
			}

			Vector2 attractionSpeed = (attractor.transform.position - transform.position).normalized * attractionVal;
			totalAttractionSpeed += attractionSpeed;
	}

		if (!isDead)
		{
			GetComponent<Rigidbody2D>().velocity = noiseSpeed + totalAttractionSpeed.normalized * speedModifier;
		}
	}

		void PlaySpawnAnimation()
	{
		TweenParams inParms = new TweenParams().SetEase(Ease.OutBounce);
		TweenParams outParms = new TweenParams().SetEase(Ease.OutCubic);
		Sequence mySequence = DOTween.Sequence();

		float startDelay = Random.Range(0.0f, StartDelay);
		mySequence.PrependInterval(startDelay);
		mySequence.AppendCallback(myCallBack);
		mySequence.Append(transform.DOScale(1.0f, 0.4f).SetAs(outParms));

		//mySequence.OnComplete(myCallBack);
	}

	public void myCallBack()
	{
		if (audioSource)
		{
			audioSource.pitch = originalPitch + Mathf.PerlinNoise1D(timeMult * Time.time) * pitchScale - pitchScale;
			audioSource.PlayOneShot(SpawnClip, 1f);
		}
	}
}

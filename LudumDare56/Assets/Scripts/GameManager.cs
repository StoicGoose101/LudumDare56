using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class GameManager : MonoBehaviour
{
    public CreatureSpawner Spawner;
	public GameObject StartUI;
	public GameObject GameOverUI;
    public GameObject WinUI;
	public GameObject PlayUI;
	public GameObject InstructionUI;
	public GameObject SurvivorUI;
	public GameObject TimeUI;
	public GameObject Light;
	public GameObject LightSource;

	public float WinTime = 30f;
	private float PlayTimer;
	public float LightMult = 0.01f;
	
	public AudioClip StartClip;
	private AudioSource audioSource;
	private float intensity;

	enum GameState
    {
		Start,
		Playing,
		GameOver,
		Win
	}

	private GameState gameState;

    void Start()
    {
		audioSource = GetComponent<AudioSource>();

		SetState(GameState.Start);
	}	

	void SetState(GameState state)
    {
		gameState = state;

		switch (gameState)
        {
            case GameState.Start:
				StartUI.SetActive(true);
				GameOverUI.SetActive(false);
				WinUI.SetActive(false);
				PlayUI.SetActive(false);
				InstructionUI.SetActive(true);
				Light.SetActive(false);
				//LightSource.SetActive(false);
				PlayTimer = WinTime;
				TimeUI.GetComponent<TMPro.TextMeshProUGUI>().text = "";
				SurvivorUI.GetComponent<TMPro.TextMeshProUGUI>().text = "";
				//LightSource.GetComponent<LightPost>().StopSound();
				audioSource.PlayOneShot(StartClip, 1.0f);
				intensity = 0.4f;
				break;

			case GameState.Playing:
				StartUI.SetActive(false);
				GameOverUI.SetActive(false);
				WinUI.SetActive(false);
				PlayUI.SetActive(true);
				InstructionUI.SetActive(false);
				Spawner.SpawnCreatures();
				Light.SetActive(true);
				//LightSource.SetActive(true);
				break;
			case GameState.GameOver:
				StartUI.SetActive(false);
				GameOverUI.SetActive(true);
				WinUI.SetActive(false);
				PlayUI.SetActive(false);
				InstructionUI.SetActive(true);
				Light.SetActive(false);
				//LightSource.SetActive(false);
				TimeUI.GetComponent<TMPro.TextMeshProUGUI>().text = "Time until dawn: " + (int)(PlayTimer) + "s";
				LightSource.GetComponent<Light2D>().intensity = 0.4f;
				LightSource.GetComponent<LightPost>().StopSound();
				break;
			case GameState.Win:
				StartUI.SetActive(false);
				GameOverUI.SetActive(false);
				PlayUI.SetActive(false);
				WinUI.SetActive(true);
				InstructionUI.SetActive(true);
				Light.SetActive(false);
				//LightSource.SetActive(false);
				GameObject[] creatures = GameObject.FindGameObjectsWithTag("Creature");
				int survivorCount = creatures.Length;

				SurvivorUI.GetComponent<TMPro.TextMeshProUGUI>().text = "Survivors: " + survivorCount;
				LightSource.GetComponent<Light2D>().intensity = 0.4f;

				break;
		}
    }

	void Update () 
	{
		switch (gameState)
		{
			case GameState.Start:
				UpdateStart();
				break;

			case GameState.Playing:
				UpdatePlaying();
				break;
			case GameState.GameOver:
				UpdateGameOver();
				break;
			case GameState.Win:
				UpdateWin();
				break;
		}
	}

	void UpdateStart()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			SetState(GameState.Playing);
        }
    }

    void UpdatePlaying()
    {
        GameObject[] creatures = GameObject.FindGameObjectsWithTag("Creature");
        if (creatures.Length == 0)
        {
            SetState(GameState.GameOver);
        }

        PlayTimer -= Time.deltaTime;
        if (PlayTimer <= 0)
        {
            SetState(GameState.Win);
        }

        intensity += Time.deltaTime * LightMult;
		LightSource.GetComponent<Light2D>().intensity = intensity;
	}

    void UpdateGameOver()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			SetState(GameState.Start);
		}
	}
	void UpdateWin()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			SetState(GameState.Start);
		}
	}
}

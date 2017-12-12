using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.Analytics;

public class GameController : MonoBehaviour
{

	#region Singleton
	private static GameController instance;

	public static GameController Instance
	{
		get
		{
			if (instance == null)
			{
				instance = FindObjectOfType<GameController> ();
			}
			return instance;
		}
	}

	#endregion

	#region Properties

	public GameObject ItemPrefab
	{
		get
		{
			return itemPrefab;
		}
	}

	#endregion

	#region Variables

	public GameObject enemy, food, health;

	public Vector3 playerStartPos;

	public float time = 0;

	private float spawnTimer = 0;

	public float spawnRate;

	public int numberOfPlayers = 2;

	public int currentNumberOfPlayers = 0;

	[SerializeField]
	private GameObject[] spawnLocations;

	[SerializeField]
	private Enemy enemyprefabs;

	[SerializeField]
	private GameObject itemPrefab;

	#endregion

	#region Pause screen variables

	public Canvas pauseScreen;

	public GameObject restartButton;

	public EventSystem eventSystem;

	public int pauseBool = 0;

	#endregion

	void Start ()
	{
		time = 0;
		Enemy.enemyCounter = 0;
		pauseScreen.enabled = false;
		Spawn ();
	}

	void Update ()
	{
		time += Time.deltaTime;
		spawnTimer += Time.deltaTime;

		Pause ();
		Spawn ();
		AllPlayerInput ();

	}

	void Spawn ()
	{
		if (spawnLocations [0] != null)
		{
			if (Enemy.enemyCounter < 6 && spawnTimer > spawnRate)
			{
				GameObject chosenSpot = spawnLocations [Random.Range (0, spawnLocations.Length)];
				Instantiate (enemyprefabs, new Vector3 (chosenSpot.transform.position.x, chosenSpot.transform.position.y), Quaternion.identity);
				spawnTimer = 0;
			}
		}
	}

	void AllPlayerInput ()
	{
		if (Input.GetButtonDown ("Pause"))
		{
			if (pauseBool == 0)
			{
				pauseBool = 1;

			} else
			{
				pauseBool = 0;

			}
		}
	}

	#region Boolean Methods
	void Pause()
	{
		// 1 = PAUSED
		// 0 = UNPAUSED
		if (pauseBool == 1) // if pausebool is true
		{
			Time.timeScale = 0; //THE GAME WAS PAUSED
			pauseScreen.enabled = true;
			eventSystem.enabled = true;


		} else //if pausebool is false
		{
			Time.timeScale = 1; //THE GAME WAS UNPAUSED
			pauseScreen.enabled = false;
			eventSystem.enabled = false;


		}
	}
	#endregion

	#region Button Methods
	public void RestartGame ()
	{
		PlayerPrefs.SetInt ("paused", pauseBool);
		SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex);

	}

	public void GotoMainMenu ()
	{
		SceneManager.LoadScene (0);
	}
	#endregion
}
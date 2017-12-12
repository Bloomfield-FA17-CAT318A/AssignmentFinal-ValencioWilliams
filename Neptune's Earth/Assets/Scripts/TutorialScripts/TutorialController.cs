using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;


public class TutorialController : MonoBehaviour
{
	[Header ("List of objects in scene")]

	public GameObject[] InGameObjects;

	public Boss neptuneScript;
	public Player playerScript;
	public Enemy enemyScript;
	public CameraShake shakeScript;
	public Transform foodSpawnLoc;
	public Transform EnemySpawnLoc;
	public EventSystem eventSystem;
	public GameObject mainMenuButton;

	#region Dialogue Stuff

	[Space]
	[Header ("Dialogue Stuff")]
	[TextArea (4, 10)]
	public List<string> dialoguePhrases;

	public Text dialogueBox;

	public int dialogueCounter = 1;

	public float dialogueCD = 4f;
	public float timeSinceLastDialogue = 7f;

	public Image loadScreenGraphic;
	public Text continueBox;

	public float spawnTimer;
	public float timeActive;

	public GameObject dialogueGameObject;

	#endregion

	#region Tutorial Actions

	[Space]
	[Header ("Tutorial stuff 1")]

	public GameObject platformPointer;
	public GameObject foodPointer;
	public GameObject ammoPointer;
	public GameObject hpPointer;
	public GameObject magPointer;
	public GameObject dpsPointer;
	public GameObject hunPointer;

	public GameObject leftPlatform;
	public GameObject rightPlatform;
	public GameObject foodPrefab;
	public GameObject enemyPrefab;
	public bool foodSpawned;

	#endregion

	#region Pause stuff

	[Space]
	[Header ("Pause the game variables")]

	public Canvas pauseScreen;

	public int pauseBool;

	#endregion

	void Start ()
	{
		Enemy.enemyCounter = 0;

		if (!dialogueGameObject.activeSelf)
		{
			dialogueGameObject.SetActive (true);
		}

		DeactivateAllGameObjects ();
		dialogueBox.text = dialoguePhrases [0];

	}

	void Update ()
	{
		timeSinceLastDialogue += Time.deltaTime;
		spawnTimer += Time.deltaTime;

		loadScreenGraphic.fillAmount += 1 / dialogueCD * Time.deltaTime; 

		AllPlayerInput ();

		SwapGraphicWithText ();

		TutorialActions ();

		PauseActions ();
	}

	void DeactivateAllGameObjects ()
	{
		pauseScreen.enabled = false;
		platformPointer.SetActive (false);
		foodPointer.SetActive (false);
		ammoPointer.SetActive (false);
		hpPointer.SetActive (false);
		magPointer.SetActive (false);
		hunPointer.SetActive (false);
		dpsPointer.SetActive (false);

		foreach (GameObject obj in InGameObjects)
		{
			obj.SetActive (false);
		}


		neptuneScript.enabled = false;
		shakeScript.enabled = false;
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


		if (Input.GetButtonDown ("Continue") || Input.GetKeyDown (KeyCode.M))
		{
			if (timeSinceLastDialogue > dialogueCD)
			{
				if (dialogueCounter < dialoguePhrases.Count)
				{
					loadScreenGraphic.fillAmount = 0;
					timeSinceLastDialogue = 0;
					dialogueBox.text = dialoguePhrases [dialogueCounter];
					dialogueCounter++;
				}
			}

		}


		if (Input.GetButtonDown ("Previous") || Input.GetKeyDown (KeyCode.N))
		{
			if (dialogueCounter != 0)
			{
				Debug.Log ("Go to previous dialogue");

				dialogueCounter--;
				dialogueBox.text = dialoguePhrases [dialogueCounter];
			}
		}
	}

	void PauseActions ()
	{
		if (pauseBool == 1) // if pausebool is true
		{
			Time.timeScale = 0; //THE GAME WAS PAUSED
			pauseScreen.enabled = true;
			eventSystem.enabled = true;
			eventSystem.firstSelectedGameObject = mainMenuButton;

		} else //if pausebool is false
		{
			Time.timeScale = 1; //THE GAME WAS UNPAUSED
			pauseScreen.enabled = false;
			eventSystem.enabled = false;


		}
	}

	void SwapGraphicWithText ()
	{
		if (loadScreenGraphic.fillAmount == 1)
		{
			loadScreenGraphic.enabled = false;
			continueBox.enabled = true;
		} else
		{
			loadScreenGraphic.enabled = true;
			continueBox.enabled = false;
		}
	}

	void TutorialActions ()
	{
		switch (dialogueCounter - 1)
		{
		case 2:
			{
				platformPointer.SetActive (true);
				leftPlatform.SetActive (true);

			}
			break;
		case 4:
			{
				platformPointer.SetActive (false);
				if (!foodSpawned)
				{
					rightPlatform.SetActive (true);
					foodPointer.SetActive (true);
					ammoPointer.SetActive (true);

					Instantiate (foodPrefab, foodSpawnLoc.position, Quaternion.identity, foodSpawnLoc);
					InGameObjects [1].SetActive (true); //Show FoodBar
					foodSpawned = true;
				}

			}
			break;
		case 5:
			{
				if (Enemy.enemyCounter < 3 && spawnTimer > 2f)
				{
					foodPointer.SetActive (false);
					ammoPointer.SetActive (false);

					GameObject thisEnemy = Instantiate (enemyPrefab, EnemySpawnLoc.position, Quaternion.identity, EnemySpawnLoc);
					thisEnemy.GetComponent<Enemy> ().enabled = true;
					spawnTimer = 0;
				}
			}
			break;
		case 7:
			{
				hpPointer.SetActive (true);
				InGameObjects [0].SetActive (true);//Show HP bar

			}
			break;
		case 8:
			{
				hpPointer.SetActive (false);
			}
			break;
		case 9:
			{
				timeActive += Time.deltaTime;
				InGameObjects [5].SetActive (true);//Show Neptune
				InGameObjects [2].SetActive (true);//Show Chaos Bar
				neptuneScript.magnitude.CurrentVal = 30f;
				neptuneScript.magnitude.MaxVal = 100f;

			}
			break;
		case 10:
			{
				magPointer.SetActive (true);
			}
			break;
		case 11:
			{
				magPointer.SetActive (false);
			}
			break;
		case 12:
			{
				hunPointer.SetActive (true);
				InGameObjects [3].SetActive (true);//Show HungerBar

			}
			break;
		case 14:
			{
				hunPointer.SetActive (false);
			}
			break;
		case 17:
			{
				dpsPointer.SetActive (true);
				InGameObjects [4].SetActive (true);//Show Dps bar
			}
			break;
		case 19:
			{
				InGameObjects [4].SetActive (false);//Hide Dps bar
				dpsPointer.SetActive (false);
			}
			break;
		case 32:
			{
				neptuneScript.enabled = true;
				shakeScript.enabled = true;
			}
			break;
		case 33:
			{
				dialogueGameObject.SetActive (false);
			}
			break;

		}//End of Switch

	}

	public void GotoMainMenu ()
	{
		SceneManager.LoadScene (0);
	}
}
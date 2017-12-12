using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;
using TMPro;

public class MenuScript : MonoBehaviour
{

	public EventSystem current;

	#region Canvas Stuff

	private float time;

	[SerializeField]
	private TMP_Text timerText;

	[SerializeField]
	private GameObject titleScreen = null, mainMenuScreen = null, loseScreen = null;

	[SerializeField]
	private GameObject tutorialButton = null, startButton = null, restartButton = null;

	[SerializeField]
	private TMP_InputField debugField;

	#endregion

	[Space]

	[Header ("Audio")]
	public AudioSource menuSound;

	public AudioSource menuButtonSound;

	public AudioClip bgm;

	public AudioClip buttons;

	void Start ()
	{
		Player.playerNum = 1;

		if (getTimer () <= 0)
		{
			StartMenu ();
		}

		if (getTimer () > 0)
		{
			GoToLoseScreen ();
			timerText.enabled = true;
			timerText.text = "You lasted: " + Mathf.Round (getTimer ()) + " secs";
		}

		if (PlayerPrefs.HasKey ("paused"))
		{
			if (getPause () == 0) //If game WAS NOT PAUSED
			{
				PlayerPrefs.DeleteKey ("paused");
			}
			if (getPause () == 0) //If game WAS PAUSED
			{
				GoToMainMenu ();
			}
		}
			

	}

	void Update ()
	{
		if (debugField.text == "HatredInstalled" && Input.GetKeyDown (KeyCode.Return))
		{
			GoToDebugRoom ();
		}
	}

	#region Start Screen Buttons

	public void GoToMainMenu ()
	{
		titleScreen.SetActive (false);
		loseScreen.SetActive (false);

		mainMenuScreen.SetActive (true);

		current.SetSelectedGameObject (tutorialButton);


	}

	public void GoToTitleScreen ()
	{
		PlayerPrefs.DeleteKey ("time");
		loseScreen.SetActive (false);
		mainMenuScreen.SetActive (false);

		titleScreen.SetActive (true);

		current.SetSelectedGameObject (startButton);

	}

	#endregion

	#region End Screen Buttons

	public void GoToLoseScreen ()
	{
		titleScreen.SetActive (false);
		mainMenuScreen.SetActive (false);

		loseScreen.SetActive (true);

		current.SetSelectedGameObject (restartButton);

	}

	#endregion

	#region Menu Buttons

	public void GoToTutorial ()
	{
		SceneManager.LoadScene (4);
	}

	public void GoToSinglePlayerMode ()
	{
		PlayerPrefs.DeleteKey ("time");
		SceneManager.LoadScene (1);
	}

	public void GoToMultiplayerMode ()
	{
		PlayerPrefs.DeleteKey ("time");
		SceneManager.LoadScene (2);
	}

	public void GoToDebugRoom ()
	{
		SceneManager.LoadScene (3);

	}

	public void QuitGame ()
	{
		Application.Quit ();
	}

	#endregion

	public void RestartButton ()
	{
		float tmp = PlayerPrefs.GetFloat ("restartScene");
		Debug.Log ("tmp is " + tmp);

		if (tmp == 1)
		{
			Debug.Log ("GO TO SINGLE");
			PlayerPrefs.DeleteKey ("restartScene");
			GoToSinglePlayerMode ();
		}
		if (tmp == 2)
		{
			Debug.Log ("GO TO MULTIPLE");

			PlayerPrefs.DeleteKey ("restartScene");
			GoToMultiplayerMode ();
		}
	}

	void StartMenu ()
	{
		timerText.enabled = false;
		loseScreen.SetActive (false);
		mainMenuScreen.SetActive (false);

		titleScreen.SetActive (true);

		current.SetSelectedGameObject (startButton);

		if (!menuSound.isPlaying)
		{
			menuSound.clip = bgm;
			menuSound.loop = true;
			menuSound.Play ();

		}

	}

	public void ButtonSound ()
	{
		//menuButtonSound.clip = buttons;
		menuSound.PlayOneShot (buttons);
	}

	#region Player Preferences

	private float getTimer ()
	{
		return PlayerPrefs.GetFloat ("time");
	}

	private int getPause ()
	{
		return PlayerPrefs.GetInt ("paused");
	}

	private int getSceneNumber ()
	{
		return PlayerPrefs.GetInt ("sceneNumber");
	}

	#endregion
}

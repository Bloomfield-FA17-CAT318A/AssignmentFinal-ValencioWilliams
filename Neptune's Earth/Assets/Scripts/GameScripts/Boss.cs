using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{
	public int restartSwitch; //Used to determine whether to restart in single player or multiplayer

	#region Magnitude Variables

	[Header ("Shake Variables")]
	public CameraShake earth;

	public Stat magnitude;

	public Stat hunger;

	public Stat dpsBar;

	public float magnitudeRate;

	public float repeatRate = 2f;

	public Enemy minions;

	#endregion

	[Space]

	#region Phase Changes
	[Header ("Phase Changes")]
	public float timeSinceLastFed = 0;

	public float enrageTime = 10f;

	public bool wasFed;

	public Animator MyAnimator;

	public AudioSource phaseSounds;

	public IEmotions currentEmotion;

	#endregion

	#region Sleep State Stuff

	public GameObject DpsMask;

	public Image dpsChecker;

	public float currentAmount;

	#endregion

	#region FadeToBlack Variables

	[Space]

	[Header ("FadeOut Variables")]
	public Image blackImage;
	public float alpha = 0f;
	public float fadeSpeed = 1.1f;
	public bool theEnd;
	public float fadeTime = 0.05f;

	#endregion

	#region Minion changes

	public List<Enemy> listOfSpawnedMinions;

	#endregion


	void Start ()
	{
		CheckScene ();

		ChangeEmotion (new HappyState ());

		if (this.enabled == true)
		{
			InvokeRepeating ("MagnitudeStarter", 2.0f, repeatRate);
		}
		magnitude.Initialize ();
		hunger.Initialize ();
		dpsBar.Initialize ();

		theEnd = false;

		blackImage.color = new Color (0, 0, 0, 0);

	}

	void Update ()
	{
		LoseGameCheck ();

		currentEmotion.Action ();
	
		timeSinceLastFed += Time.deltaTime;

		if (timeSinceLastFed > 2f)
		{
			hunger.CurrentVal = 0;
		}

		if (dpsChecker.isActiveAndEnabled)
		{
			currentAmount = CalculateFillAmount (dpsBar.CurrentVal, 0, dpsBar.MaxVal , 0, 1);

			dpsChecker.fillAmount = Mathf.Lerp (dpsChecker.fillAmount, currentAmount, Time.deltaTime * 5f);

		}
	}

	void FixedUpdate ()
	{
		foreach (Enemy minions in FindObjectsOfType<Enemy> ())
		{
			if (!listOfSpawnedMinions.Contains (minions))
			{
				listOfSpawnedMinions.Add (minions);
			}

			if (minions.IsDead)
			{
				listOfSpawnedMinions.Remove (minions);
			}
		}
			


	}

	public void MagnitudeStarter ()
	{
		magnitude.CurrentVal += magnitudeRate;

		if (theEnd)
		{
			earth.shakeAmount = 5f;
			repeatRate = 0.1f;
		}
	}

	void LoseGameCheck ()
	{
		if (magnitude.CurrentVal >= magnitude.MaxVal)
		{
			theEnd = true;
		}

		if (theEnd)
		{
			fadeScreen (fadeTime);

			if (alpha >= 1f)
			{
				if (GameController.Instance != null)
				{
					PlayerPrefs.SetFloat ("time", GameController.Instance.time);
					PlayerPrefs.SetFloat ("restartScene", restartSwitch);
					AnalyticsTracking.Instance.OnEnd ();
				}

				SceneManager.LoadScene (0);

			}
		}
	}

	void fadeScreen (float fade)
	{
		alpha += fade * fadeSpeed * Time.deltaTime;

		alpha = Mathf.Clamp01 (alpha);

		blackImage.color = new Color (0, 0, 0, alpha);
				
		//yield return new WaitForSeconds (fade);


	}

	public void ChangeEmotion (IEmotions newEmotion)
	{
		if (currentEmotion != null)
		{
			currentEmotion.End ();
		}

		currentEmotion = newEmotion;

		currentEmotion.Enter (this);

	}

	void OnTriggerEnter2D (Collider2D other)
	{
		if (other.tag == "Projectile" && !wasFed)
		{
			Projectile temp = other.GetComponent<Projectile> ();

			wasFed = true;

			magnitude.CurrentVal -= temp.magDmg;

			timeSinceLastFed = 0;

			hunger.CurrentVal += temp.hungerDmg;

			StartCoroutine (FedTimer ());

			if (DpsMask.activeSelf)
			{
				dpsBar.CurrentVal += temp.dpsDmg;
			}

		}
	}

	void CheckScene()
	{
		if (SceneManager.GetActiveScene ().name == "Single Player Mode")
		{
			restartSwitch = 1;
		}

		if (SceneManager.GetActiveScene ().name == "Multi-Player Mode")
		{
			restartSwitch = 2;
		}
	}

	#region Advanced Calculations

	float CalculateFillAmount (float val, float inMin, float inMax, float outMin, float outMax)
	{
		return ((val - inMin) * (outMax - outMin)) / ((inMax - inMin) + outMin);

	}

	#endregion

	#region IEnumerator

	IEnumerator FedTimer ()
	{
		yield return new WaitForSeconds (0.01f);
		wasFed = false;

	}

	#endregion
}
